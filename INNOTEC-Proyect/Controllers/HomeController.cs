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

    class NestedConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Departamento) || objectType == typeof(Categorium) || objectType == typeof(Subcategorium) || objectType == typeof(Producto);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject item = JObject.Load(reader);

            if (objectType == typeof(Producto))
            {
                int productId = item["idProductos"]?.ToObject<int>() ?? 0;
                string productName = item["nombre"]?.ToString() ?? "Nombre no disponible";
                string productDescription = item["descripcionDelProducto"]?.ToString();
                int productPrice = item["precio"]?.ToObject<int>() ?? 0;
                int? productQuantity = item["cantidad"]?.ToObject<int?>();
                byte[] productImage = item["imagenDelProducto"]?.ToObject<byte[]>();

                return new Producto
                {
                    IdProductos = productId,
                    Nombre = productName,
                    DescripcionDelProducto = productDescription,
                    Precio = productPrice,
                    Cantidad = productQuantity,
                    ImagenDelProducto = productImage,
                };
            }
            else if (objectType == typeof(Departamento))
            {
                return new Departamento
                {
                    IdDepartamento = item["departamentoId"].ToObject<int>(),
                    Nombre = item["departamentoNombre"].ToString(),
                    Productos = item["productos"].Select(p => DeserializeProducto(p)).ToList(),
                    Categoria = item["categorias"].ToObject<List<Categorium>>(serializer)
                };
            }
            else if (objectType == typeof(Categorium))
            {
                return new Categorium
                {
                    IdCategoria = item["categoriaId"].ToObject<int>(),
                    Nombre = item["categoriaNombre"].ToString(),
                    Productos = item["productos"].Select(p => DeserializeProducto(p)).ToList(),
                    Subcategoria = item["subcategorias"].ToObject<List<Subcategorium>>(serializer)
                };
            }
            else if (objectType == typeof(Subcategorium))
            {
                return new Subcategorium
                {
                    IdSubcategoria = item["subcategoriaId"].ToObject<int>(),
                    Nombre = item["subcategoriaNombre"].ToString(),
                    Productos = item["productos"].Select(p => DeserializeProducto(p)).ToList()
                };
            }

            return null;


            throw new NotImplementedException("Unrecognized type");
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException("Unneeded because we only read the JSON");
        }

        private Producto DeserializeProducto(JToken item)
        {
            return new Producto
            {
                IdProductos = item["productoId"]?.ToObject<int>() ?? 0,
                Nombre = item["nombre"]?.ToString() ?? "Nombre no disponible",
            };
        }
    }


}
