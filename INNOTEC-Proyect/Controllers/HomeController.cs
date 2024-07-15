using INNOTEC_Proyect.Clases;
using INNOTEC_Proyect.Models;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace INNOTEC_Proyect.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _environment = hostingEnvironment;
            _httpClientFactory = httpClientFactory;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<List<ML.Producto>> GetAll()
        {
            List<ML.Producto> productos = new List<ML.Producto>();
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.GetAsync("Producto/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    productos = JsonConvert.DeserializeObject<List<ML.Producto>>(resultString, new JsonSerializerSettings
                    {
                        Converters = new List<JsonConverter> { new NestedConverter() }
                    });
                }
                return productos;
            }
        }

        [HttpGet]
        public async Task<List<Departamento>> GetnavMenu()
        {
            List<Departamento> menuItems = new List<Departamento>();
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.GetAsync("Departamento/GetMenu");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    JsonSerializerSettings settings = new JsonSerializerSettings { Converters = new List<JsonConverter> { new NestedConverter() } };
                    menuItems = JsonConvert.DeserializeObject<List<Departamento>>(jsonString, settings);
                }
            }
            return menuItems;
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string search)

        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string urlAPI = $"{_configuration["UrlAPI"]}Producto/GetByName?product={Uri.EscapeDataString(search)}";
                var response = await client.GetAsync(urlAPI);
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    var productos = JsonConvert.DeserializeObject<List<ML.Producto>>(resultString);
                    return Json(productos.Select(p => new { p.IdProductos, p.Nombre }));
                }
                return Json(new { error = true, message = "No data found" });
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message });
            }
        }


        [HttpGet("OneProduct/{id}")]
        public async Task<IActionResult> OneProduct(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                string urlAPI = $"{_configuration["UrlAPI"]}Producto/GetById?id={id}";
                var response = await client.GetAsync(urlAPI);

                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    var producto = JsonConvert.DeserializeObject<ML.Producto>(resultString);

                    if (producto != null)
                    {
                        var menuItems = await GetnavMenu();

                        var viewModel = new HomeViewModel
                        {

                            MenuItems = menuItems,
                            Producto = producto
                        };

                        return View("~/Views/Products/OneProduct.cshtml", viewModel);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = true, message = ex.Message });
            }
        }

        private async Task<List<Departamento>> GetMenuItemsAsync()
        {

            return new List<Departamento>();

        }
        public async Task<IActionResult> Index()
        {
            var menuItems = await GetnavMenu();
            var productos = await GetAll();

            var viewModel = new HomeViewModel
            {
                MenuItems = menuItems,
                Productos = productos
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            ViewData["Title"] = "Privacy Policy";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}