using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cidean.GatherHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Cidean.GatherHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("Username","Password")] SignInModel signInModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            if (!(signInModel.Username == "admin" && signInModel.Password == "pass"))
            {
                ModelState.AddModelError(string.Empty, "Invalid Username and/or Password.");
                signInModel.IsValid = false;
                return View(signInModel);
            }

            if (signInModel.Username == "admin" && signInModel.Password == "pass")
            {



                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "admin", ClaimValueTypes.String, "GatherHub")
                };

                var identity = new ClaimsIdentity(claims, "Password");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                        AllowRefresh = false,
                        IsPersistent = false
                    });

                
                return RedirectToAction("Index", "Home");
                
            }
            
            

            return RedirectToAction(nameof(Denied));
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }


        public IActionResult Denied()
        {
            return View();
        }


    }




}