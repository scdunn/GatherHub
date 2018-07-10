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

namespace Cidean.GatherHub.Controllers
{
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
        public async Task<IActionResult> SignIn([Bind("Username","Password")] SignInModel signInModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            Member user = _work.Members.GetAll().SingleOrDefault(m => m.EmailAddress == signInModel.Username);
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
                    new Claim(ClaimTypes.Name, user.EmailAddress , ClaimValueTypes.String, "GatherHub")
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(SignIn));
        }


        public IActionResult Denied()
        {
            return View();
        }


    }




}