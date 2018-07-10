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
    public class MembersController : Controller
    {

        private readonly IUnitOfWork _work;
        

        public MembersController(IUnitOfWork work)
        {
            _work = work;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _work.Members.GetAll().ToListAsync());
        }


        public async Task<IActionResult> Create()
        {
            return View(nameof(Edit), new Member());
        }
                

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var member = await _work.Members.GetById(id.Value);

            if (member == null)
                return NotFound();
            
            return View(member);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id","EmailAddress","TempPassword","FirstName","LastName")] Member member)
        {
            

            if (id != member.Id)
                return NotFound();

            //only set password if value exists
            if (!String.IsNullOrWhiteSpace(member.TempPassword))
                member.SetPassword(member.TempPassword);


            if (ModelState.IsValid)
            {
                try
                {
                    if (member.Id == 0)
                        //new member
                        _work.Members.Insert(member);
                    else
                        //existing member
                        _work.Members.Update(member);
                    
                    //save
                    await _work.Save();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!memberExists(member.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(member);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var member = await _work.Members.GetById(id.Value);

            if (member == null)
                return NotFound();

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _work.Members.GetById(id);
            _work.Members.Delete(member);
            await _work.Save();

            return RedirectToAction(nameof(Index));
        }

        private bool memberExists(int id)
        {
            return (_work.Members.GetById(id).Result != null);

        }
    }
}
