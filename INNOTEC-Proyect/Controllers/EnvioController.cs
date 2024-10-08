﻿using INNOTEC_Proyect.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;



namespace INNOTEC_Proyect.Controllers
{
    public class EnvioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory;
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public EnvioController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _environment = hostingEnvironment;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index(string idCompra, string idsCompra, decimal? total, int? cantidad)
        {
            List<int> listaIdsCompra = new List<int>();

            if (!string.IsNullOrEmpty(idCompra))
            {
                if (int.TryParse(idCompra, out int parsedId))
                {
                    listaIdsCompra.Add(parsedId);
                }
            }

            if (!string.IsNullOrEmpty(idsCompra))
            {
                listaIdsCompra.AddRange(idsCompra.Split(',').Select(id =>
                {
                    int.TryParse(id, out int parsedId);
                    return parsedId;
                }).Where(id => id != 0).ToList());
            }

            Cantidad = cantidad ?? 0;
            Total = total ?? 0;

            var pedidos = listaIdsCompra.Select(id => new Pedido
            {
                IdCompra = id
            }).ToList();

            var viewModel = new EnvioPedidoViewModel
            {
                Envios = listaIdsCompra.Select(id => new Envio { IdCompra = id }).ToList(),
                Pedidos = pedidos,
                Total = total ?? 0
            };

            // Guardar valores en la sesión
            HttpContext.Session.SetInt32("Cantidad", Cantidad);
            HttpContext.Session.SetString("Total", Total.ToString());
            HttpContext.Session.SetString("Pedidos", JsonConvert.SerializeObject(pedidos));

            ViewBag.PublicKey = _configuration["MercadoPago:PublicKey"];
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> InsertEnvioPedido([FromBody] EnvioPedidoViewModel model)
        {
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);

                // Insertar Envio
                var responseEnvio = await client.PostAsJsonAsync("envio/Insert", model.Envio);
                if (!responseEnvio.IsSuccessStatusCode)
                {
                    return BadRequest("Error al insertar el envío.");
                }

                // Insertar Pedido
                var responsePedido = await client.PostAsJsonAsync("envio/Insert", model.Pedido);
                if (!responsePedido.IsSuccessStatusCode)
                {
                    return BadRequest("Error al insertar el pedido.");
                }

                return Ok("Envío y Pedido creados con éxito.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEnvios(string searchId)
        {
            List<Envio> envios = new List<Envio>();
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.GetAsync("Envio/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    envios = JsonConvert.DeserializeObject<List<Envio>>(resultString);
                }
                else
                {
                    // Manejar el caso donde la respuesta no es exitosa
                    return View("Error", new ErrorViewModel { RequestId = "Error al obtener los envíos" });
                }
            }

            ViewBag.SearchId = searchId;  // Pasar el valor de searchId a la vista
            return View("Gestion_Envios", envios);
        }



        [HttpGet]
        public async Task<IActionResult> GetEnviosByUserId()
        {
            List<Envio> envios = new List<Envio>();
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userIdClaim == null)
                {
                    return Unauthorized(new { success = false, message = "User not authenticated." });
                }

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return BadRequest(new { success = false, message = "Invalid User ID." });
                }

                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);
                    var response = await client.GetAsync($"Envio/GetById?id={userId}");
                    if (response.IsSuccessStatusCode)
                    {
                        var resultString = await response.Content.ReadAsStringAsync();
                        envios = JsonConvert.DeserializeObject<List<Envio>>(resultString);
                    }
                }

                if (envios == null || !envios.Any())
                {
                    return NotFound(new { success = false, message = "No envios found for the user." });
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                return StatusCode(500, new { success = false, message = "Internal server error.", details = ex.Message });
            }
            return Ok(envios);
        }

        [HttpPost]
        public async Task<IActionResult> InsertEnvio([FromBody] Envio envio)
        {
       
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(new { success = false, message = "User not authenticated." });
            }

            if (int.TryParse(userIdClaim, out int userId))
            {
                envio.UsuarioId = userId;
            }
            else
            {
                return BadRequest(new { success = false, message = "Invalid User ID." });
            }

            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.PostAsJsonAsync("envio/Insert", envio);
                if (response.IsSuccessStatusCode)
                {
                    return Ok(new { success = true });
                }
                return BadRequest(new { success = false });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEnvio(int id, Envio envio)
        {
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.PutAsJsonAsync($"Envio/Update?id={id}", envio);
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEnvio(int id)
        {
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.DeleteAsync($"envio/Delete?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertPedido([FromBody] Pedido pedido)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (userIdClaim == null)
            {
                return Unauthorized(new { success = false, message = "User not authenticated." });
            }

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return BadRequest(new { success = false, message = "Invalid User ID." });
            }


            Cantidad = HttpContext.Session.GetInt32("Cantidad") ?? 0;
            Total = decimal.Parse(HttpContext.Session.GetString("Total"));
            var pedidos = JsonConvert.DeserializeObject<List<Pedido>>(HttpContext.Session.GetString("Pedidos"));

            // Asignar valores necesarios
            if (pedidos != null && pedidos.Any())
            {

                string urlAPI = _configuration["UrlAPI"]; // Asegúrate de que esta clave esté en tu appsettings.json

                foreach (var individualPedido in pedidos)
                {
                    individualPedido.UsuarioId = userId;
                    individualPedido.FechaPedido = DateTime.Now;
                    individualPedido.EstadoPedido = "Entrante";
                    individualPedido.IdEnvio = pedido.IdEnvio;


                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(urlAPI);
                        var response = await client.PostAsJsonAsync("Pedido/Insert", individualPedido);
                        if (!response.IsSuccessStatusCode)
                        {
                            return BadRequest(new { success = false, message = $"Error al insertar el pedido con IdCompra {individualPedido.IdCompra}." });
                        }
                    }
                }
            }
            else
            {
                return BadRequest(new { success = false, message = "No orders found in the request." });
            }

            return Ok(new { success = true });
        }


        [HttpPost]
        [Route("Envio/create_Preference")]
        public async Task<IActionResult> CreatePreference([FromBody] OrderData orderData)
        {
            try
            {
                var accessToken = _configuration["MercadoPago:AccessToken"];
                MercadoPagoConfig.AccessToken = accessToken;

                // Recuperar valores desde la sesión
                Cantidad = HttpContext.Session.GetInt32("Cantidad") ?? 0;
                Total = decimal.Parse(HttpContext.Session.GetString("Total"));

                decimal unitario = Total / Cantidad;

                // Crea el objeto de request de la preference
                var request = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = orderData.Title,
                    Quantity = Cantidad,
                    CurrencyId = "MXN",
                    UnitPrice = unitario,
                },
            }
                };

                var client = new PreferenceClient();
                Preference preference = await client.CreateAsync(request);

                bool isCreated = !string.IsNullOrEmpty(preference.Id);

                if (isCreated)
                {
                    return Ok(new { success = true, id = preference.Id });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Error al crear la preferencia de pago." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPedidos()
        {
            List<Pedido> pedidos = new List<Pedido>();
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.GetAsync("Pedido/GetAll");
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    pedidos = JsonConvert.DeserializeObject<List<Pedido>>(resultString);
                }
                else
                {
                    return View("Error", new ErrorViewModel { RequestId = "Error al obtener los pedidos" });
                }
            }

            return View("Gestion_Pedidos", pedidos);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.GetAsync($"Pedido/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    var pedido = JsonConvert.DeserializeObject<Pedido>(resultString);
                    return Ok(pedido);
                }
                return BadRequest("Error al obtener el pedido.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePedido([FromBody] Pedido pedido)
        {
            if (pedido == null)
            {
                return BadRequest("Datos del pedido no válidos.");
            }

            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);

                var response = await client.PutAsJsonAsync($"Pedido/Update", pedido);

                if (response.IsSuccessStatusCode)
                {
                    return Ok("Pedido actualizado con éxito.");
                }
                return BadRequest("Error al actualizar el pedido.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePedido(int id)
        {
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.DeleteAsync($"Pedido/Delete?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                return BadRequest();
            }
        }



        public class OrderData
        {
            public string? Title { get; set; }
        }
    }
}
