using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Geoprofs.Models;
using Geoprofs.Models.Data;
using Newtonsoft.Json;

namespace Geoprofs.Controllers
{
    public class CoworkersController : Controller
    {

        #region --database connectie
        private readonly DB _context;

        public CoworkersController(DB context)
        {
            _context = context;
        }
        #endregion

        #region --planning pagina
        public async Task<IActionResult> Index()
        {
            var Data = _context.coworkers
            .Include(s => s.Position)
            .Include(a => a.AbsenceRequest);

            return View(await Data.ToListAsync());
        }

        #endregion

        #region --verlof pagina
        public async Task<IActionResult> Details(int? id)
        {
            //verlof aanvragen ophalen
            if (id == null)
            {
                return NotFound();
            }
            var updatedview = await _context.coworkers
            .Include(s => s.Position)
            .Include(a => a.AbsenceRequest)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.coworkerId == id);
            // verlof types ophalen
            var absenceTypes = JsonConvert.SerializeObject(_context.absenceTypes.ToList());

            TempData["abs"] = absenceTypes;
            if (updatedview == null)
            {
                return NotFound();
            }

            return View(updatedview);
        }
        #endregion

    }
}
