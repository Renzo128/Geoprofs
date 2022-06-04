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
            return View(await _context.absenceRequests.ToListAsync());
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
                return RedirectToAction(nameof(Index));
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

        // GET: AbsenceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: AbsenceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var absenceRequest = await _context.absenceRequests.FindAsync(id);
            _context.absenceRequests.Remove(absenceRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbsenceRequestExists(int id)
        {
            return _context.absenceRequests.Any(e => e.absenceId == id);
        }
    }
}
