using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ML;
using INNOTEC_Proyect.Models;
using Microsoft.Extensions.Configuration;
using INNOTEC_Proyect.Clases;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Capturista(string activeTab = "#departamentos-tab")
        {
            var viewModel = new HomeViewModel
            {
                MenuItems = await _httpClient.GetFromJsonAsync<List<Departamento>>("Departamento/GetAllDepto"),
                Categorias = await _httpClient.GetFromJsonAsync<List<Categorium>>("Categoria/GetAll"),
                Subcategorias = await _httpClient.GetFromJsonAsync<List<Subcategorium>>("Subcategoria/GetAll"),
                Productos = await _httpClient.GetFromJsonAsync<List<Producto>>("Producto/GetAll"),
                Proveedores = await _httpClient.GetFromJsonAsync<List<Proveedor>>("Proveedor/GetAll"),
                ActiveTab = activeTab
            };

            return View("~/Views/Admin/Capturista.cshtml", viewModel);
        }

        // Create methods
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
        public async Task<IActionResult> CreateProducto([FromForm] ML.Producto producto, IFormFile ImagenDelProducto)
        {
            if (ImagenDelProducto != null && ImagenDelProducto.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await ImagenDelProducto.CopyToAsync(ms);
                    producto.ImagenDelProducto = ms.ToArray();
                }
            }

            var response = await _httpClient.PostAsJsonAsync("Producto/Insert", producto);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al agregar el producto" });
        }



        // Update methods
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

        [HttpPost]  // Cambiar a POST para manejar multipart/form-data
        public async Task<IActionResult> UpdateProducto(int id, [FromForm] ML.Producto producto, IFormFile ImagenDelProducto)
        {
            if (ImagenDelProducto != null && ImagenDelProducto.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await ImagenDelProducto.CopyToAsync(ms);
                    producto.ImagenDelProducto = ms.ToArray();
                }
            }

            var response = await _httpClient.PutAsJsonAsync($"Producto/Update?id={id}", producto);
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Error al actualizar el producto" });
        }

        // Delete methods
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


        // GetById methods
        [HttpGet]
        public async Task<IActionResult> GetDepartamentoById(int id)
        {
            var departamento = await _httpClient.GetFromJsonAsync<Departamento>($"Departamento/GetByIdDepto?id={id}");
            if (departamento != null)
            {
                return Json(departamento);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetProveedorById(int id)
        {
            var proveedor = await _httpClient.GetFromJsonAsync<Proveedor>($"Proveedor/GetById?id={id}");
            if (proveedor != null)
            {
                return Json(proveedor);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriaById(int id)
        {
            var categoria = await _httpClient.GetFromJsonAsync<Categorium>($"Categoria/GetById?id={id}");
            if (categoria != null)
            {
                return Json(categoria);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetSubcategoriaById(int id)
        {
            var subcategoria = await _httpClient.GetFromJsonAsync<Subcategorium>($"Subcategoria/GetById?id={id}");
            if (subcategoria != null)
            {
                return Json(subcategoria);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductoById(int id)
        {
            var response = await _httpClient.GetStringAsync($"Producto/GetById?id={id}");
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new NestedConverter());

            var producto = JsonConvert.DeserializeObject<Producto>(response, settings);

            if (producto != null)
            {
                return Json(producto);
            }
            return Json(null);
        }

    }
}
