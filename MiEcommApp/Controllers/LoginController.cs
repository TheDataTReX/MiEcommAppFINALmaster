using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using MiEcommApp.Helpers;
using System.Data;
using System.Security.Claims;
using MiEcommApp.Models;
using Newtonsoft.Json;

namespace MiEcommApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly NorthwindContext _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(NorthwindContext context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string cEmail, string cPassword)
        {
            var userInfo = await (from emp in _context.Employees
                                  where emp.Email == cEmail
                                  select new
                                  {
                                      IDEmployee = emp.EmployeeId,
                                      Nombre = emp.FirstName,
                                      Apellido = emp.LastName,
                                      Email = emp.Email,
                                      Password = emp.Password,
                                      IDsPermiso = emp.Idpermisos.Select(x => x.Idpermiso),
                                      NombrePermisos = emp.Idpermisos.Select(x => x.Descripcion)

                                  }).SingleOrDefaultAsync();

            if (userInfo != null)
            {
                if (userInfo != null && Argon2PasswordHasher.VerifyHashedPassword(userInfo.Password,cPassword))
                {

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.IDEmployee.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, userInfo.IDEmployee.ToString()),
                        new Claim(ClaimTypes.GivenName, userInfo.Nombre.ToString()),
                        new Claim(ClaimTypes.Surname, userInfo.Apellido.ToString()),
                        new Claim(ClaimTypes.Email, userInfo.Email.ToString()),
                    };


                    var allPermisos = userInfo.IDsPermiso;

                    foreach (var permiso in allPermisos)
                    {
                        claims.Add(new Claim("Permiso", permiso.ToString()));
                    }
                    
                    
                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                    await HttpContext.SignInAsync(
                        "CookieAuth",
                        new ClaimsPrincipal(claimsIdentity));

                    _logger.LogInformation("User: {} successfully logged in", userInfo.Email);

                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorMessage"] = "Email o Contraseña Incorrectos";
                return RedirectToAction("Index", "Login");
            }
            TempData["ErrorMessage"] = "Email o Contraseña Incorrectos";
            return RedirectToAction("Index", "Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }


    }
}

