using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cidean.GatherHub.Core.Helpers;
using Cidean.GatherHub.Core.Models;
using Cidean.GatherHub.Core.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Cidean.GatherHub.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUnitOfWork _work;
        private readonly Mailer _mailer;


        public RegisterController(IUnitOfWork work, Mailer mailer)
        {
            _work = work;
            _mailer = mailer;
        }


        private Registration GetRegistration()
        {
            var registration = HttpContext.Session.Get<Registration>("REG");
            if (registration == null)
            {
                registration = new Registration();
                if (User.Identity.IsAuthenticated)
                {
                    registration.MemberId = Int32.Parse(User.Claims.Single(m => m.Type == "id").Value);
                }
                _work.Logger.Log($"New Registration created.");
            }
    

            return registration;
        }

        private void SetRegistration(Registration registration)
        {
            HttpContext.Session.Set<Registration>("REG", registration);
        }


        [Authorize]
        public IActionResult Index()
        {
            var registration = GetRegistration();
            return View(registration);
        }

        [Authorize]
        public async Task<IActionResult> Course(int? id)
        {

            if (id == null)
                return NotFound();
        

            var registration = GetRegistration();
            
            //check for existing course registration to prevent duplicates
            if (registration.Courses.Any(p => p.Id == id.Value))
            {
                TempData["CourseMessage"] = "You all already registered for this course.";
                return RedirectToAction(nameof(Index));
            }

            var course = _work.Courses.GetAll()
                .Include(p => p.Instructor)
                .Single(p => p.Id == id.Value);

            registration.Courses.Add(course);

            SetRegistration(registration);


            _work.Logger.Log($"Course: {course.Title} added to registration. ");

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Remove(int? id)
        {
            if (id == null)
                return NotFound();

            var registration = GetRegistration();
            registration.Courses.Remove(registration.Courses.Single(m => m.Id == id.Value));
            HttpContext.Session.Set<Registration>("REG", registration);

            return RedirectToAction(nameof(Index));

        }

        [Authorize]
        public async Task<IActionResult> Review()
        {
            var registration = GetRegistration();
            var member = await _work.Members.GetById(registration.MemberId);

            foreach (var course in registration.Courses)
            {
                //only add course for member if not already registered
                if (!member.CourseMembers.Any(m => m.CourseId == course.Id))
                    member.AddCourse(course.Id);
            }

            return View(registration);
        }

        [Authorize]
        public async Task<IActionResult> Complete()
        {   
            var registration = GetRegistration();
            var member = await _work.Members.GetById(registration.MemberId);
            string coursesEmail = "";

            foreach (var course in registration.Courses)
            {
                //only add course for member if not already registered
                if (!member.CourseMembers.Any(m => m.CourseId == course.Id))
                {
                    member.AddCourse(course.Id);
                    coursesEmail += "<li>" + course.Title + "</li>";
                }
            }

            await _work.Save();

            var tags = new Dictionary<string, string>();
            tags.Add("FIRST_NAME", member.FirstName);
            tags.Add("COURSES", coursesEmail);

            await _mailer.SendMail("register", member.EmailAddress, member.FullName, tags);


            HttpContext.Session.Remove("REG");

            return RedirectToAction(nameof(Confirmation));
        }

        [Authorize]
        public IActionResult Confirmation()
        {
            return View();
        }

    }
}