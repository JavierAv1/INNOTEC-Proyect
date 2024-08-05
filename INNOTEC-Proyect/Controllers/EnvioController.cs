using INNOTEC_Proyect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MercadoPago.Config;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using ML;

namespace INNOTEC_Proyect.Controllers
{
    public class EnvioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpClientFactory _httpClientFactory;

        public EnvioController(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _environment = hostingEnvironment;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index(string idCompra, string idsCompra, decimal? total)
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

            var viewModel = new EnvioPedidoViewModel
            {
                Envios = listaIdsCompra.Select(id => new Envio { IdCompra = id }).ToList(),
                Pedidos = listaIdsCompra.Select(id => new Pedido { IdCompra = id }).ToList(),
                Total = total ?? 0
            };

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

        // Otros métodos de Envio y Pedido...

        [HttpGet]
        public async Task<List<Envio>> GetAllEnvios()
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
                return envios;
            }
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
            // Obtener el User ID de los claims
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

        [HttpGet]
        public async Task<IActionResult> CreatePreference(int addressId)
        {
            var preferenceRequest = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Title = "Compra en INNOTEC",
                        Quantity = 1,
                        CurrencyId = "MXN",
                        UnitPrice = 1.00m // Reemplazar con el precio real
                    }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = "Test",
                    Surname = "User",
                    Email = "test_user_123456@testuser.com"
                }
            };

            MercadoPagoConfig.AccessToken = _configuration["MercadoPago:AccessToken"];
            var client = new PreferenceClient();
            var preference = await client.CreateAsync(preferenceRequest);

            if (preference == null)
            {
                return BadRequest(new { success = false, message = "No se pudo crear la preferencia de pago." });
            }

            return Ok(new { preferenceId = preference.Id });
        }

        [HttpPost]
        public async Task<IActionResult> InsertPedido([FromBody] HomeViewModel model)
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

            var pedido = new Pedido
            {
                IdCompra = model.IdCompra,
                FechaPedido = DateTime.Now,
                UsuarioId = userId
            };

            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.PostAsJsonAsync("Pedido/Insert", pedido);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { success = false, message = "Error al insertar el pedido." });
                }
            }

            return Ok(new { success = true });
        }
    }
}
