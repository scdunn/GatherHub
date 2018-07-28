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

        public async Task<IActionResult> Index()
        {
            var memberid = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst("id").Value);
            var member = await _work.Members.GetById(memberid);
            return View(member);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("EmailAddress", "FirstName", "LastName", "TempPassword", "ReturnUrl")] Member memberModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }


            var memberid = Int32.Parse(((ClaimsIdentity)User.Identity).FindFirst("id").Value);
            var member = await _work.Members.GetById(memberid);

            member.EmailAddress = memberModel.EmailAddress;
            member.FirstName = memberModel.FirstName;
            member.LastName = memberModel.LastName;
           
            _work.Members.Update(member);
            await _work.Save();
            

            return RedirectToAction("Index", "Account");


        }




        public IActionResult SignIn(string returnUrl)
        {
            var signInModel = new MemberSignInModel() { ReturnUrl = returnUrl };

            return View(signInModel);
        }

        private async Task DoSignIn(Member member)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, member.EmailAddress , ClaimValueTypes.String, "GatherHub"),
                    new Claim("id", member.Id.ToString() , ClaimValueTypes.String, "GatherHub")
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
        }


        public IActionResult CreateAccount(string returnUrl)
        {
            var model = new MemberCreateModel() { ReturnUrl = returnUrl };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount([Bind("EmailAddress","FirstName","LastName","Password", "ReturnUrl")] MemberCreateModel createModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }


            Member member = new Member();
            member.EmailAddress = createModel.EmailAddress;
            member.FirstName = createModel.FirstName;
            member.LastName = createModel.LastName;
            member.SetPassword(createModel.Password);


            _work.Members.Insert(member);
            await _work.Save();

            _work.Logger.Log($"Sign In for {member.EmailAddress}");

            await DoSignIn(member);

            if (!string.IsNullOrEmpty(createModel.ReturnUrl))
                return Redirect(createModel.ReturnUrl);

            return RedirectToAction("Index", "Home");

            
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn([Bind("EmailAddress", "Password", "ReturnUrl")] MemberSignInModel signInModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Member member = _work.Members.GetAll().SingleOrDefault(m => m.EmailAddress == signInModel.EmailAddress);
            bool isValid = true;
            if (member == null)
                isValid = false;
            else if (!member.IsValidPassword(signInModel.Password))
                isValid = false;

            //invalid sign in
            if (!isValid)
            {
                _work.Logger.Log($"Sign In Failure.");
                ModelState.AddModelError(string.Empty, "Invalid Username and/or Password.");
                signInModel.IsValid = false;
                return View(signInModel);
            }

            //valid sign in
            if (isValid)
            {

                _work.Logger.Log($"Sign In for {member.EmailAddress}");

                await DoSignIn(member);

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