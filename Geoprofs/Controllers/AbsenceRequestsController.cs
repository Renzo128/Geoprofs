using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geoprofs.Models;
using Geoprofs.Models.Data;
using Microsoft.Extensions.Primitives;

namespace Geoprofs.Controllers
{
    public class AbsenceRequestsController : Controller
    {
        private readonly DB _context;

        public AbsenceRequestsController(DB context)
        {
            _context = context;
        }

        // GET: AbsenceRequests
        public async Task<IActionResult> Index()
        {
            var Data = _context.absenceRequests
            .Include(s => s.coworker)
            .Include(t => t.AbsenceType);
            return View(await Data.ToListAsync());
        }

        // GET: AbsenceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absenceRequest = await _context.absenceRequests
                .FirstOrDefaultAsync(m => m.absenceId == id);
            if (absenceRequest == null)
            {
                return NotFound();
            }

            return View(absenceRequest);
        }

        // GET: AbsenceRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AbsenceRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("absenceId,Coworker,AbsenceStart,AbsenceEnd,Note,absenceType,absenceStatus")] AbsenceRequest absenceRequest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(absenceRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("user_id") });
            }
            return View(absenceRequest);
        }

        // GET: AbsenceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absenceRequest = await _context.absenceRequests.FindAsync(id);
            if (absenceRequest == null)
            {
                return NotFound();
            }
            return View(absenceRequest);
        }

        // POST: AbsenceRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("absenceId,Coworker,AbsenceStart,AbsenceEnd,Note,absenceType,absenceStatus")] AbsenceRequest absenceRequest)
        {
            if (id != absenceRequest.absenceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(absenceRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbsenceRequestExists(absenceRequest.absenceId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(absenceRequest);
        }
        public async Task<IActionResult> edit_Accept(string sender, string id)
        {
            string coworker = Request.Form["Coworker_" + id.ToString()];
            int newId = Convert.ToInt32(id);
            foreach (string key in Request.Form.Keys)
            {
                if (key.Contains(id))
                {
                    string absencerequest = Convert.ToString(Request.Form[key].ToString());
                    var absencerequests = new AbsenceRequest() { absenceId = newId, absenceStatus = absencerequest };

                    using (var context = _context)
                    {
                        context.absenceRequests.Attach(absencerequests);
                        context.Entry(absencerequests).Property(x => x.absenceStatus).IsModified = true;
                        context.SaveChanges();
                        return RedirectToAction(nameof(Index));

                    }
                }
            }



            return RedirectToAction(nameof(Index));
        }

        // GET: AbsenceRequests/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            //string coworker = Request.Form["Coworker_" + id.ToString()];
            int newId = Convert.ToInt32(id);


            foreach (string key in Request.Form.Keys)
            {
                if (key.Contains(id))
                {
                    string absencerequest = Convert.ToString(Request.Form[key].ToString());
                    var absencerequests = new AbsenceRequest() { absenceId = newId, absenceStatus = absencerequest };

                    using (var context = _context)
                    {
                        context.absenceRequests.Attach(absencerequests);
                        context.absenceRequests.Remove(absencerequests);
                        context.SaveChanges();

                        return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("user_id") });


                    }
                }
            }



            return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("user_id") });
        }

        // POST: AbsenceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        /*     public async Task<IActionResult> DeleteConfirmed(int id)
             {
                 var absenceRequest = await _context.absenceRequests.FindAsync(id);
                 _context.absenceRequests.Remove(absenceRequest);
                 await _context.SaveChangesAsync();
                 return RedirectToAction(nameof(Index));
             } */

        private bool AbsenceRequestExists(int id)
        {
            return _context.absenceRequests.Any(e => e.absenceId == id);
        }


        public async Task<IActionResult> PastMonth(int month, int year)
        {
            month--;

            if (month == 0)
            {
                month = 12;
                year--;
            }
            TempData["Month"] = month;
            TempData["Year"] = year;
            return RedirectToAction("index", "Coworkers");

        }

        public async Task<IActionResult> NextMonth(int month, int year)
        {
            month++;

            if (month == 13)
            {
                month = 1;
                year = (int)year + 1;
            }
            TempData["Month"] = month;
            TempData["Year"] = year;
            return RedirectToAction("index", "Coworkers");

        }

    public async Task<IActionResult> success(List<int> arr)
        {
            foreach (var item in arr)
            {
                var absencerequests = new AbsenceRequest() { absenceId = item, absenceStatus = "Geaccepteerd" };


                    _context.absenceRequests.Attach(absencerequests);
                    _context.Entry(absencerequests).Property(x => x.absenceStatus).IsModified = true;
            }
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> rejected(List<int> arr)
        {
            foreach (var item in arr)
            {
                var absencerequests = new AbsenceRequest() { absenceId = item, absenceStatus = "Geweigerd" };


                _context.absenceRequests.Attach(absencerequests);
                _context.Entry(absencerequests).Property(x => x.absenceStatus).IsModified = true;
            }
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));

        }
    }
}
