using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cidean.GatherHub.Core.Data;
using Cidean.GatherHub.Core.Models;
using Cidean.GatherHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Cidean.GatherHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _work;

        public AuthController(IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("Username","Password")] AdminSignInModel signInModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            AdminUser user = _work.AdminUsers.GetAll().SingleOrDefault(m => m.Username == signInModel.Username);
            bool isValid = true;
            if (user == null)
                isValid = false;
            else if(!user.IsValidPassword(signInModel.Password))
                isValid = false;

            //invalid sign in
            if(!isValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid Username and/or Password.");
                signInModel.IsValid = false;
                return View(signInModel);
            }

            //valid sign in
            if (isValid)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username , ClaimValueTypes.String, "GatherHub")
                };

                var identity = new ClaimsIdentity(claims, "Password");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("admin",
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
            await HttpContext.SignOutAsync("admin");
            return RedirectToAction(nameof(SignIn));
        }


        public IActionResult Denied()
        {
            return View();
        }


    }




}