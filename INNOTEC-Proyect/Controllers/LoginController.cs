using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using System;

namespace INNOTEC_Proyect.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public LoginController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userNameOrEmail, string Contraseña)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);

                    var response = await httpClient.GetAsync($"Usuario/Login?userNameOrEmail={userNameOrEmail}&password={Contraseña}");

                    if (response.IsSuccessStatusCode)
                    {
                        var readContent = await response.Content.ReadAsStringAsync();

                        var apiResponse = JsonConvert.DeserializeObject<ML.Result>(readContent);

                        if (apiResponse != null && apiResponse.Success)
                        {
                            var usuario = JsonConvert.DeserializeObject<ML.Usuario>(apiResponse.Object.ToString());
                            result.Object = usuario;
                            result.Success = true;

                           
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, usuario.UserName),
                                new Claim("FullName", usuario.Nombre),
                                new Claim(ClaimTypes.Role, usuario.TipoUsuarioIdTipousuario.ToString()),
                                new Claim("UserId", usuario.UsuarioId.ToString()),
                                new Claim("TipoUsuarioId", usuario.TipoUsuarioIdTipousuario.ToString())

                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                           
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = true,
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                            };

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            result.ErrorMessage = apiResponse?.ErrorMessage ?? "Error desconocido.";
                            result.Success = false;
                        }
                    }
                    else
                    {
                        result.ErrorMessage = await response.Content.ReadAsStringAsync();
                        result.Success = false;
                    }
                }
            }
            catch (Exception error)
            {
                result.Success = false;
                result.ErrorMessage = error.Message;
            }

            if (result.Success)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = "Contraseña o UserName incorrecto ";
                return PartialView("Modal");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    string urlAPI = _configuration["UrlAPI"];
                    httpClient.BaseAddress = new Uri(urlAPI);
                  
                   
                    var jsonContent = JsonConvert.SerializeObject(new
                    {
                        //usuario.UsuarioId,
                        usuario.Nombre,
                        usuario.ApellidoPaterno,
                        usuario.ApellidoMaterno,
                        usuario.FechaDeNacimiento,
                        usuario.Sexo,
                        usuario.UserName,
                        usuario.Correo,
                        usuario.Contraseña,
                        usuario.Telefono,
                        usuario.Celular,
                        usuario.TipoUsuarioIdTipousuario,
                        usuario.FotoDePerfil
                    });

                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("Usuario/Insert", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var readContent = await response.Content.ReadAsStringAsync();
                        result.Success = JsonConvert.DeserializeObject<bool>(readContent);
                        if (result.Success)
                        {
                            return RedirectToAction("Login"); 
                        }
                    }
                    else
                    {
                        var readContent = await response.Content.ReadAsStringAsync();
                        result.ErrorMessage = JsonConvert.DeserializeObject<string>(readContent);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            ViewBag.ErrorMessage = "Registration failed: " + result.ErrorMessage;
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

