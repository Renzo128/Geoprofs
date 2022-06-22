using Microsoft.AspNetCore.Mvc;
using Geoprofs.Models;
using Geoprofs.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Geoprofs.Controllers
{
    public class LoginController : Controller
    {

        #region --Database connectie
        private readonly DB _context;

        public LoginController(DB context)  // database data ophalen
        {
            _context = context;
        }
        #endregion

        #region --login verifieren
        [HttpPost]
        public async Task<ActionResult> Verify(string Password, string Username)
        {   // controlleren of gebruiker de goede gebruikersnaam en wachtwoord gebruikt
            var Data = _context.logins
                .Where(l => l.Username == Username && l.Password == Password);
            if (Data.Any())
            {
                var otherData = _context.logins.Where(x => x.Password == Password && x.Username == Username).FirstOrDefault();

                var userdata = _context.coworkers.Where(x => x.coworkerId == otherData.Coworker).FirstOrDefault();

                TempData["user_id"] = otherData.Coworker;
                TempData["username"] = Username;
                TempData["password"] = Password;
                TempData["supervisor"] = userdata.supervisor;
                TempData["role"] = userdata.position;

                var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand").Count();

                TempData["Requests"] = requests;
                // aantal verlof dagen bij elkaar optellen
                var users = _context.absenceRequests.Where(x => x.coworker == userdata);
                int allVacation = userdata.vacationdays;
                foreach(var item in users)
                {
                    var hours = (item.AbsenceEnd - item.AbsenceStart).TotalHours;
                    int days = (int) hours / 24;
                    allVacation = allVacation - days;
                }
                TempData["absenceDays"] = allVacation;

                return RedirectToAction("index", "Coworkers");
            }
            else
            {
                return Content("Failed");
            }

        }
        #endregion


    }
}
