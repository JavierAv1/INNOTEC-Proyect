using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ML;
using INNOTEC_Proyect.Models;
using Microsoft.Extensions.Configuration;

namespace INNOTEC_Proyect.Controllers
{
    public class CapturistaController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CapturistaController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["UrlAPI"]);
        }

        [HttpGet]
        public async Task<IActionResult> Capturista()
        {
            var viewModel = new HomeViewModel
            {
                MenuItems = await _httpClient.GetFromJsonAsync<List<Departamento>>("Departamento/GetAllDepto"),
                Categorias = await _httpClient.GetFromJsonAsync<List<Categorium>>("Categoria/GetAll"),
                Subcategorias = await _httpClient.GetFromJsonAsync<List<Subcategorium>>("Subcategoria/GetAll"),
                Productos = await _httpClient.GetFromJsonAsync<List<Producto>>("Producto/GetAll"),
                Proveedores = await _httpClient.GetFromJsonAsync<List<Proveedor>>("Proveedor/GetAll")
            };

            return View("~/Views/Admin/Capturista.cshtml", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartamento([FromBody] Departamento departamento)
        {
            var response = await _httpClient.PostAsJsonAsync("Departamento/InsertDepto", departamento);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al agregar el departamento" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProveedor([FromBody] Proveedor proveedor)
        {
            var response = await _httpClient.PostAsJsonAsync("Proveedor/Insert", proveedor);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al agregar el proveedor" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoria([FromBody] Categorium categoria)
        {
            var response = await _httpClient.PostAsJsonAsync("Categoria/Insert", categoria);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al agregar la categoría" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubcategoria([FromBody] Subcategorium subcategoria)
        {
            var response = await _httpClient.PostAsJsonAsync("Subcategoria/Insert", subcategoria);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al agregar la subcategoría" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] Producto producto)
        {
            var response = await _httpClient.PostAsJsonAsync("Producto/Insert", producto);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al agregar el producto" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartamento(int id, [FromBody] Departamento departamento)
        {
            var response = await _httpClient.PutAsJsonAsync($"Departamento/UpdateDepto?id={id}", departamento);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al actualizar el departamento" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProveedor(int id, [FromBody] Proveedor proveedor)
        {
            var response = await _httpClient.PutAsJsonAsync($"Proveedor/Update?id={id}", proveedor);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al actualizar el proveedor" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategoria(int id, [FromBody] Categorium categoria)
        {
            var response = await _httpClient.PutAsJsonAsync($"Categoria/Update?id={id}", categoria);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al actualizar la categoría" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubcategoria(int id, [FromBody] Subcategorium subcategoria)
        {
            var response = await _httpClient.PutAsJsonAsync($"Subcategoria/Update?id={id}", subcategoria);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al actualizar la subcategoría" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] Producto producto)
        {
            var response = await _httpClient.PutAsJsonAsync($"Producto/Update?id={id}", producto);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al actualizar el producto" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartamento(int id)
        {
            var response = await _httpClient.DeleteAsync($"Departamento/DeleteDepto?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al eliminar el departamento" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            var response = await _httpClient.DeleteAsync($"Proveedor/Delete?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al eliminar el proveedor" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var response = await _httpClient.DeleteAsync($"Categoria/Delete?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al eliminar la categoría" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteSubcategoria(int id)
        {
            var response = await _httpClient.DeleteAsync($"Subcategoria/Delete?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al eliminar la subcategoría" });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var response = await _httpClient.DeleteAsync($"Producto/Delete?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al eliminar el producto" });
        }
    }
}
