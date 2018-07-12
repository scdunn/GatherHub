using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cidean.GatherHub.Core.Data;
using Cidean.GatherHub.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Cidean.GatherHub.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "admin")]
    public class CoursesController : Controller
    {

        private readonly IUnitOfWork _work;

        private async Task SetViewBag()
        {

            ViewBag.CourseCategories = new SelectList(await _work.CourseCategories.GetAll().ToListAsync(), "Id", "Title");
            ViewBag.Instructors = new SelectList(await _work.Members.GetAll().ToListAsync(), "Id","FullName");

        }

        public CoursesController(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _work.Courses.GetAll()
                .Include(m=>m.CourseMembers)
                .ThenInclude(m=>m.Member)
                .ToListAsync());
        }


        public async Task<IActionResult> Create()
        {
            await SetViewBag();
            return View(nameof(Edit), new Course());
        }
                

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _work.Courses.GetById(id.Value);

            if (course == null)
                return NotFound();

            await SetViewBag();
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,MeetingTimes,CourseCategoryId,InstructorId")] Course course)
        {

            if (id != course.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (course.Id == 0)
                        //new course
                        _work.Courses.Insert(course);
                    else
                        //existing course
                        _work.Courses.Update(course);
                    
                    //save
                    await _work.Save();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            await SetViewBag();
            return View(course);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var course = await _work.Courses.GetById(id.Value);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _work.Courses.GetById(id);
            _work.Courses.Delete(course);
            await _work.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_work.Courses.GetById(id).Result != null);

        }
    }
}
