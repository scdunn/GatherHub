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
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _work;

        public AccountController(IUnitOfWork work)
        {
            _work = work;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignIn(string returnUrl)
        {
            var signInModel = new SignInModel() { ReturnUrl = returnUrl };

            return View(signInModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("Username","Password", "ReturnUrl")] SignInModel signInModel)
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
                _work.Logger.Log($"Sign In Failure.");
                ModelState.AddModelError(string.Empty, "Invalid Username and/or Password.");
                signInModel.IsValid = false;
                return View(signInModel);
            }

            //valid sign in
            if (isValid)
            {

                _work.Logger.Log($"Sign In for {user.EmailAddress}");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.EmailAddress , ClaimValueTypes.String, "GatherHub"),
                    new Claim("id", user.Id.ToString() , ClaimValueTypes.String, "GatherHub")
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

                if (!string.IsNullOrEmpty(signInModel.ReturnUrl))
                    return Redirect(signInModel.ReturnUrl);

                return RedirectToAction("Index", "Home");
                
            }
            
            

            return RedirectToAction(nameof(Denied));
        }

        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _work.Logger.Log($"Sign out for {User.Identity.Name}");
            return RedirectToAction(nameof(SignIn));
        }


        public IActionResult Denied()
        {
            return View();
        }


        public IActionResult Courses()
        {

            var memberid = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst("id").Value);


            var courses = _work.Courses.GetMemberCourses(memberid).Include(m => m.Instructor).Include(m => m.Category);

            return View(courses);


        }
    }




}