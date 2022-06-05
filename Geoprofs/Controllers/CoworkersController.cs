using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geoprofs.Models;
using Geoprofs.Models.Data;

namespace Geoprofs.Controllers
{
    public class CoworkersController : Controller
    {
        private readonly DB _context;

        public CoworkersController(DB context)
        {
            _context = context;
        }

        // GET: Coworkers
        public async Task<IActionResult> Index()
        {
            var Data = _context.coworkers
            .Include(s => s.Position)
            .Include(a => a.AbsenceRequest);

            return View(await Data.ToListAsync());
        }

        // GET: Coworkers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var updatedview = await _context.coworkers
            .Include(s => s.Position)
            .Include(a => a.AbsenceRequest)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.coworkerId == id);
  /*          var coworker = await _context.coworkers
                .FirstOrDefaultAsync(m => m.coworkerId == id); */
            if (updatedview == null)
            {
                return NotFound();
            }

            return View(updatedview);
        }

        // GET: Coworkers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coworkers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("coworkerId,CoworkerName,coworkerLastname,bsn,position,supervisor,startDate,absence,vacationdays")] Coworker coworker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coworker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coworker);
        }

        // GET: Coworkers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coworker = await _context.coworkers.FindAsync(id);
            if (coworker == null)
            {
                return NotFound();
            }
            return View(coworker);
        }

        // POST: Coworkers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("coworkerId,CoworkerName,coworkerLastname,bsn,position,supervisor,startDate,absence,vacationdays")] Coworker coworker)
        {
            if (id != coworker.coworkerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coworker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoworkerExists(coworker.coworkerId))
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
            return View(coworker);
        }

        // GET: Coworkers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coworker = await _context.coworkers
                .FirstOrDefaultAsync(m => m.coworkerId == id);
            if (coworker == null)
            {
                return NotFound();
            }

            return View(coworker);
        }

        // POST: Coworkers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coworker = await _context.coworkers.FindAsync(id);
            _context.coworkers.Remove(coworker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoworkerExists(int id)
        {
            return _context.coworkers.Any(e => e.coworkerId == id);
        } 
    }
}
