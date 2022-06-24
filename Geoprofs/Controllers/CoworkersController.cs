﻿using System;
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
            TempData.Keep("role");
            if (TempData["role"] != null)
            {
                var Data = _context.coworkers
                .Include(s => s.Position)
                .Include(a => a.AbsenceRequest);

                return View(await Data.ToListAsync());
            }
			else
			{
                return RedirectToAction("index", "Home");
            }
        }

        #endregion

        #region --verlof pagina
        public async Task<IActionResult> Details(int? id)
        {
            TempData.Keep("role");
            if (TempData["role"] != null)
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

                var userdata = _context.coworkers.Where(x => x.coworkerId == (int)TempData["user_id"]).FirstOrDefault();
                var users = _context.absenceRequests.Where(x => x.coworker == userdata);
                TempData.Keep("user_id");

                //overige verlof optellen
                int allVacation = userdata.vacationdays;
                foreach (var item in users)
                {
                    if (item.absenceStatus == "Geaccepteerd")
                    {
                        while (item.AbsenceEnd != item.AbsenceStart)
                        {
                            
                            int weekend = (int)item.AbsenceStart.DayOfWeek;
                            if (weekend != 6 && weekend != 0)
                            {
                                allVacation = allVacation - 1;
                            }
                            item.AbsenceStart = item.AbsenceStart.AddDays(1);
                        }
                    }
                }
                TempData["absenceDays"] = allVacation;


                return View(updatedview);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
        #endregion

    }
}
