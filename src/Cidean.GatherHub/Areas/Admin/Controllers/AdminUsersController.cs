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
    public class AdminUsersController : Controller
    {

        private readonly IUnitOfWork _work;
        

        public AdminUsersController(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _work.AdminUsers.GetAll().ToListAsync());
        }


        public async Task<IActionResult> Create()
        {
            return View(nameof(Edit), new AdminUser());
        }
                

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _work.AdminUsers.GetById(id.Value);

            if (user == null)
                return NotFound();
            
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id","Username","TempPassword","FirstName","LastName")] AdminUser user)
        {
            

            if (id != user.Id)
                return NotFound();

            //only set password if value exists
            if (!String.IsNullOrWhiteSpace(user.TempPassword))
                user.SetPassword(user.TempPassword);


            if (ModelState.IsValid)
            {
                try
                {
                    if (user.Id == 0)
                        //new user
                        _work.AdminUsers.Insert(user);
                    else
                        //existing user
                        _work.AdminUsers.Update(user);
                    
                    //save
                    await _work.Save();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(user);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var user = await _work.AdminUsers.GetById(id.Value);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _work.AdminUsers.GetById(id);
            _work.AdminUsers.Delete(user);
            await _work.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_work.AdminUsers.GetById(id).Result != null);

        }
    }
}
