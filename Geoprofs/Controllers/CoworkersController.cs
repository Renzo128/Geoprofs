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

            TempData["teamAbsence1"] = getPercentage(1);
            TempData["teamAbsence2"] = getPercentage(2);
            TempData["teamAbsence3"] = getPercentage(3);
            TempData["teamAbsence4"] = getPercentage(4);
            TempData["teamAbsence5"] = getPercentage(5);
            TempData["teamAbsence6"] = getPercentage(6);
            TempData["teamAbsence7"] = getPercentage(7);
            @TempData.Keep("role");
            if (TempData["role"] != null)
            {
                var data = _context.coworkers
                .Include(s => s.Position)
                .Include(a => a.AbsenceRequest);

                return View(await data.ToListAsync());
            }
			else
			{
                return RedirectToAction("index", "Home");
            }
        }

        private String getPercentage(int job)
        {

            double data = _context.coworkers.Where(x => x.position == job).Count();
            int allVacation = 0;
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);




            var userdata = _context.coworkers.Where(x => x.position == job);
            foreach (Coworker group in userdata)
            {
                var users = _context.absenceRequests.Where(x => x.coworker == group && x.AbsenceStart >= startDate && x.AbsenceEnd <=endDate);

                //overige verlof optellen
                foreach (var item in users)
                {
                    if (item.absenceStatus == "Geaccepteerd")
                    {
                        while (item.AbsenceEnd != item.AbsenceStart)
                        {

                            int weekend = (int)item.AbsenceStart.DayOfWeek;
                            if (weekend != 6 && weekend != 0)
                            {
                                allVacation = allVacation + 1;
                            }
                            item.AbsenceStart = item.AbsenceStart.AddDays(1);
                        }
                    }
                }
            }
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            double days = DateTime.DaysInMonth(year, month);

            double allVacations = ((allVacation / data) / days) *100;
            String vacation = String.Format("{0:0.00}", allVacations);



            return vacation;

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
                var updatedView = await _context.coworkers
                .Include(s => s.Position)
                .Include(a => a.AbsenceRequest)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.coworkerId == id);
                // verlof types ophalen
                var absenceTypes = JsonConvert.SerializeObject(_context.absenceTypes.ToList());

                TempData["abs"] = absenceTypes;
                if (updatedView == null)
                {
                    return NotFound();
                }

                var userData = _context.coworkers.Where(x => x.coworkerId == (int)TempData["userId"]).FirstOrDefault();
                var users = _context.absenceRequests.Where(x => x.coworker == userData);
                TempData.Keep("user_id");

                //overige verlof optellen
                int allVacation = userData.vacationdays;
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


                return View(updatedView);
            }
            else
            {
                return RedirectToAction("index", "Home");
            }
        }
        #endregion

    }
}
