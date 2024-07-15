using INNOTEC_Proyect.Models;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;
using INNOTEC_Proyect.Clases;

namespace INNOTEC_Proyect.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;  

        public ProductController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _environment = hostingEnvironment;
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

        [HttpGet]
        public async Task<IActionResult> Products(int page = 1, int pageSize = 8)
        {

            var productos = await GetAll();
            var menuItems = await GetnavMenu();

            var paginatedProducts = productos.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            var viewModel = new HomeViewModel
            {
                MenuItems = menuItems,
                Productos = paginatedProducts,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(productos.Count / (double)pageSize)

            };

            return View("~/Views/Products/Products.cshtml", viewModel);  
        }
    }
}
