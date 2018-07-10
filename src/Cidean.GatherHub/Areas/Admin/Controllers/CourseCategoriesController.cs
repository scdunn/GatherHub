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
    public class CourseCategoriesController : Controller
    {

        private readonly IUnitOfWork _work;


        public CourseCategoriesController(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _work.CourseCategories.GetAll().ToListAsync());
        }


        public async Task<IActionResult> Create()
        {
            return View(nameof(Edit), new CourseCategory());
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var courseCategory = await _work.CourseCategories.GetById(id.Value);

            if (courseCategory == null)
                return NotFound();

            return View(courseCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title")] CourseCategory courseCategory)
        {

            if (id != courseCategory.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (courseCategory.Id == 0)
                        //new course
                        _work.CourseCategories.Insert(courseCategory);
                    else
                        //existing course
                        _work.CourseCategories.Update(courseCategory);

                    //save
                    await _work.Save();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseCategoryExists(courseCategory.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }


            return View(courseCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var courseCategory = await _work.CourseCategories.GetById(id.Value);

            if (courseCategory == null)
                return NotFound();

            return View(courseCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseCategory = await _work.CourseCategories.GetById(id);
            _work.CourseCategories.Delete(courseCategory);
            await _work.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool CourseCategoryExists(int id)
        {
            return (_work.CourseCategories.GetById(id).Result != null);

        }
    }
}
