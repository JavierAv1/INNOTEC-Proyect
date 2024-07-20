using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ML;

namespace INNOTEC_Proyect.Controllers
{
    [Route("Usuario")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AdminController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["UrlAPI"]);
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _httpClient.GetFromJsonAsync<List<Usuario>>("Usuario/GetAll");
            return View(usuarios);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _httpClient.GetFromJsonAsync<Usuario>($"Usuario/GetById?id={id}");
            if (usuario != null)
            {
                return Json(new { success = true, data = usuario });
            }
            return Json(new { success = false });
        }

        [HttpPost("Insert")]
        public async Task<IActionResult> CreateUsuario(Usuario usuario)
        {
            var response = await _httpClient.PostAsJsonAsync("Usuario/Insert", usuario);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUsuario(int id, Usuario usuario)
        {
            var response = await _httpClient.PutAsJsonAsync($"Usuario/Update?id={id}", usuario);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var response = await _httpClient.DeleteAsync($"Usuario/Delete?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
