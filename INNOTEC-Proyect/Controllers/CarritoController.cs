using INNOTEC_Proyect.Clases;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace INNOTEC_Proyect.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IConfiguration _configuration;

        public CarritoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> AddCarrito(int idProducto)
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Json(new { success = false, message = "Usuario no autenticado" });
            }

            int idUsuario = int.Parse(userIdClaim.Value);

            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);

                    var requestUrl = $"Compra/Insert?idUsuario={idUsuario}&idProducto={idProducto}";
                    var response = await httpClient.PostAsync(requestUrl, null);

                    var readContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result.Success = bool.TryParse(readContent, out bool isSuccess) && isSuccess;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = readContent;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return Json(new { success = result.Success, message = result.Success ? "Producto agregado correctamente" : result.ErrorMessage });
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserId(int idUsuario)
        {
            List<Compra> compras = new List<Compra>();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);

                    var response = await httpClient.GetAsync($"Compra/GetByUserId?id={idUsuario}");
                    var readContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonArray = JArray.Parse(readContent);
                        compras = jsonArray.Select(json => new Compra
                        {
                            IdCompra = (int)json["idCompra"],
                            Cantidad = (int)json["cantidad"],
                            FechaDeCompra = DateOnly.Parse(json["fechaDeCompra"].ToString()),
                            FechaVencimiento = DateOnly.Parse(json["fechaVencimiento"].ToString()),
                            Idproducto = (int)json["idProducto"],
                            IdproductoNavigation = new Producto
                            {
                                Nombre = (string)json["nombreProducto"],
                                DescripcionDelProducto = (string)json["descripcionProducto"],
                                Precio = json["precio"] != null ? (int)json["precio"] : 0,
                                ImagenDelProducto = json["imagenProducto"] != null ? Convert.FromBase64String(json["imagenProducto"].ToString()) : null
                            }
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo del error
            }

            return View(compras);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.GetAsync("Compra/GetAll");
                    var readContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var compras = JsonConvert.DeserializeObject<List<Compra>>(readContent);
                        result.Results = compras.Cast<object>().ToList();
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = readContent;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return View(result.Results);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.GetAsync($"Compra/GetById?id={id}");
                    var readContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result.Object = JsonConvert.DeserializeObject<Compra>(readContent);
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = readContent;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return View(result.Object);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Compra compra)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonContent = JsonConvert.SerializeObject(compra);
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                    var response = await httpClient.PutAsync("Compra/Update", content);
                    var readContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = readContent;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await httpClient.DeleteAsync($"Compra/Delete?id={id}");
                    var readContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.ErrorMessage = readContent;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return Json(result);
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
    }
}
