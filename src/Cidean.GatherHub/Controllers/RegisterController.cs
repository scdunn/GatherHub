using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cidean.GatherHub.Core.Helpers;
using Cidean.GatherHub.Core.Models;
using Cidean.GatherHub.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace Cidean.GatherHub.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUnitOfWork _work;


        public RegisterController(IUnitOfWork work)
        {
            _work = work;
        }


        private Registration GetRegistration()
        {
            var registration = HttpContext.Session.Get<Registration>("REG");
            if (registration == null)
                registration = new Registration();

            return registration;
        }

        private void SetRegistration(Registration registration)
        {
            HttpContext.Session.Set<Registration>("REG", registration);
        }


        public IActionResult Index()
        {
            var registration = GetRegistration();
            return View(registration);
        }

        
        public async Task<IActionResult> Course(int? id)
        {

            if (id == null)
                return NotFound();

            var registration = GetRegistration();
            
            //check for existing course registration to prevent duplicates
            if (registration.Courses.Any(p => p.Id == id.Value))
                return RedirectToAction(nameof(Index));

            var course = _work.Courses.GetAll()
                .Include(p => p.Instructor)
                .Single(p => p.Id == id.Value);

            registration.Courses.Add(course);

            SetRegistration(registration);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Remove(int? id)
        {
            if (id == null)
                return NotFound();

            var registration = GetRegistration();
            registration.Courses.Remove(registration.Courses.Single(m => m.Id == id.Value));
            HttpContext.Session.Set<Registration>("REG", registration);

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Verify()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(SignIn), "Auth", new { returnUrl = "~/register/verify" });

            return RedirectToAction(nameof(Complete));
        }


        public async Task<IActionResult> Complete()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(SignIn), "Auth",new {returnUrl="~/register/verify" });

            var memberId = Int32.Parse(User.Claims.Single(m => m.Type == "id").Value);
            var registration = GetRegistration();
            var member = await _work.Members.GetById(memberId);

            foreach (var course in registration.Courses)
                member.AddCourse(course.Id);

           await _work.Save();

            HttpContext.Session.Remove("REG");

            return RedirectToAction(nameof(Confirmation));
        }


        public IActionResult Confirmation()
        {
            return View();
        }

    }
}