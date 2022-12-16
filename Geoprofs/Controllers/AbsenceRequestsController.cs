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
        #region --database connectie
        private readonly DB _context;

        public AbsenceRequestsController(DB context)
        {
            _context = context;
        }
        #endregion

        #region --verlof goedkeuren pagina
        public async Task<IActionResult> Index()
        {
            TempData.Keep("role");
            if (TempData["role"] != null)
            {
                if ((int)TempData["role"] >= 6)
                {
                    var data = _context.absenceRequests //verlof aanvragen ophalen
                    .Include(s => s.coworker)
                    .Include(t => t.AbsenceType);
                    var supervisor = (int)TempData["isSupervisor"];
                    TempData.Keep("isSupervisor");
                    var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();
                    //totaal aantal open aanvragen ophalen
                    TempData["requests"] = requests;


                    return View(await data.ToListAsync());
                }
				else
				{
                    return RedirectToAction("index", "Home");

                }
            }
            else
            {
                return RedirectToAction("index", "Home"); 
            }
            }
        #endregion

        #region --verlof aanvragen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("absenceId,Coworker,AbsenceStart,AbsenceEnd,Note,absenceType,absenceStatus")] AbsenceRequest absenceRequest)
        {
         //verlof aanvraag aanmaken
            if (ModelState.IsValid)
            {
                _context.Add(absenceRequest);
                await _context.SaveChangesAsync();
                if (TempData["isSupervisor"] != null)
                {
                    var supervisor = (int)TempData["isSupervisor"];
                    TempData.Keep("isSupervisor");
                    var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();

                    TempData["requests"] = requests;
                }
                var userdata = _context.coworkers.Where(x => x.coworkerId == (int)TempData["userId"]).FirstOrDefault();
                var users = _context.absenceRequests.Where(x => x.coworker == userdata);
                TempData.Keep("userId");
                //totaal verlof dagen
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


                return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("userId") });
            }
            return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("userId") });

        }
        #endregion

        #region --verlof goedkeuren of afwijzen
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> editAccept(string id)// string sender
        {
            //verlof aanvraag goedkeuren/ afwijzen
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
                        var supervisor = (int)TempData["isSupervisor"];
                        TempData.Keep("isSupervisor");
                        var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();
                        //totaal aantal aanvragen die nog openstaand zijn ophalen
                        TempData["Requests"] = requests;
                        var userdata = _context.coworkers.Where(x => x.coworkerId == (int)TempData["userId"]).FirstOrDefault();
                        var users = _context.absenceRequests.Where(x => x.coworker == userdata);
                        TempData.Keep("userId");

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

                        return RedirectToAction(nameof(Index));

                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> success(List<int> arr)
        {
            //verlof aanvragen goedkeuren

                foreach (var item in arr)
                {
                    var absencerequests = new AbsenceRequest() { absenceId = item, absenceStatus = "Geaccepteerd" };


                    _context.absenceRequests.Attach(absencerequests);
                    _context.Entry(absencerequests).Property(x => x.absenceStatus).IsModified = true;
                }
                _context.SaveChanges();
            TempData.Keep("isSupervisor");

            var supervisor = (int)TempData["isSupervisor"];
            TempData.Keep("isSupervisor");
            var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();
            //verlof aanvragen die openstaan zijn ophalen
            TempData["requests"] = requests;
            var userData = _context.coworkers.Where(x => x.coworkerId == (int)TempData["userId"]).FirstOrDefault();
            var users = _context.absenceRequests.Where(x => x.coworker == userData);
            TempData.Keep("userId");

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



            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> rejected(List<int> arr)
        {
            //verlof aanvragen afwijzen

            foreach (var item in arr)
            {
                var absencerequests = new AbsenceRequest() { absenceId = item, absenceStatus = "Geweigerd" };


                _context.absenceRequests.Attach(absencerequests);
                _context.Entry(absencerequests).Property(x => x.absenceStatus).IsModified = true;
            }
            _context.SaveChanges();

            var supervisor = (int)TempData["isSupervisor"];
            TempData.Keep("isSupervisor");
            var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();
            //totaal aantal open aanvragen ophalen
            TempData["requests"] = requests;
            var userData = _context.coworkers.Where(x => x.coworkerId == (int)TempData["userId"]).FirstOrDefault();
            var users = _context.absenceRequests.Where(x => x.coworker == userData);
            TempData.Keep("userId");

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

            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region --Verlof aanvraag verwijderen
        public async Task<IActionResult> Delete(string id)
        {
            //verlof verwijderen
            int newId = Convert.ToInt32(id);


            foreach (string key in Request.Form.Keys)
            {
                if (key.Contains(id))
                {
                    string absenceRequest = Convert.ToString(Request.Form[key].ToString());
                    var absenceRequests = new AbsenceRequest() { absenceId = newId, absenceStatus = absenceRequest };

                    using (var context = _context)
                    {
                        context.absenceRequests.Attach(absenceRequests);
                        context.absenceRequests.Remove(absenceRequests);
                        context.SaveChanges();

                        var supervisor = (int)TempData["isSupervisor"];
                        TempData.Keep("isSupervisor");
                        var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();

                        TempData["requests"] = requests;
                        var userData = _context.coworkers.Where(x => x.coworkerId == (int)TempData["userId"]).FirstOrDefault();
                        var users = _context.absenceRequests.Where(x => x.coworker == userData);
                        TempData.Keep("userId");

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



                        return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("userId") });


                    }
                }
            }
            return RedirectToAction("Details", "Coworkers", new { id = TempData.Peek("userId") });
        }
        #endregion

        #region --kalender

        public async Task<IActionResult> PastMonth(int month, int year)
        {
            //naar vorige maand gaan
            month--;

            if (month == 0)
            {
                month = 12;
                year--;
            }
            TempData["month"] = month;
            TempData["year"] = year;
            TempData.Keep("supervisor");
            return RedirectToAction("index", "Coworkers");

        }

        public async Task<IActionResult> NextMonth(int month, int year)
        {
            //naar volgende maand gaan
            month++;

            if (month == 13)
            {
                month = 1;
                year = (int)year + 1;
            }
            TempData["month"] = month;
            TempData["year"] = year;
            TempData.Keep("supervisor");

            return RedirectToAction("index", "Coworkers");

        }
        #endregion

        #region --reload
        public async Task<IActionResult> reload()
        {
            // pagina herladen
            var supervisor = (int)TempData["isSupervisor"];
            TempData.Keep("isSupervisor");
            var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();

            TempData["requests"] = requests;
            return RedirectToAction(nameof(Index));


        }
        #endregion
    }
}
