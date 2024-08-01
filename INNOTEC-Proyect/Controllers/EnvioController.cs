using INNOTEC_Proyect.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using INNOTEC_Proyect.Clases;
using ML;
using System.Security.Claims;

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
        public async Task<Envio> GetEnvioById(int id)
        {
            Envio envio = null;
            string urlAPI = _configuration["UrlAPI"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlAPI);
                var response = await client.GetAsync($"Envio/GetById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    envio = JsonConvert.DeserializeObject<Envio>(resultString);
                }
                return envio;
            }
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

        //[HttpGet]
        //public async Task<List<Pedido>> GetAllPedidos()
        //{
        //    List<Pedido> pedidos = new List<Pedido>();
        //    string urlAPI = _configuration["UrlAPI"];
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);
        //        var response = await client.GetAsync("envio/GetAll");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var resultString = await response.Content.ReadAsStringAsync();
        //            pedidos = JsonConvert.DeserializeObject<List<Pedido>>(resultString);
        //        }
        //        return pedidos;
        //    }
        //}

        //[HttpGet]
        //public async Task<Pedido> GetPedidoById(int id)
        //{
        //    Pedido pedido = null;
        //    string urlAPI = _configuration["UrlAPI"];
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);
        //        var response = await client.GetAsync($"envio/GetById?id={id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var resultString = await response.Content.ReadAsStringAsync();
        //            pedido = JsonConvert.DeserializeObject<Pedido>(resultString);
        //        }
        //        return pedido;
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> InsertPedido(Pedido pedido)
        //{
        //    string urlAPI = _configuration["UrlAPI"];
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);
        //        var response = await client.PostAsJsonAsync("envio/Insert", pedido);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Ok();
        //        }
        //        return BadRequest();
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdatePedido(int id, Pedido pedido)
        //{
        //    string urlAPI = _configuration["UrlAPI"];
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);
        //        var response = await client.PutAsJsonAsync($"envio/UpdatePedido?id={id}", pedido);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Ok();
        //        }
        //        return BadRequest();
        //    }
        //}

        //[HttpDelete]
        //public async Task<IActionResult> DeletePedido(int id)
        //{
        //    string urlAPI = _configuration["UrlAPI"];
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(urlAPI);
        //        var response = await client.DeleteAsync($"envio/DeletePedido?id={id}");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Ok();
        //        }
        //        return BadRequest();
        //    }
        //}
    }
}
