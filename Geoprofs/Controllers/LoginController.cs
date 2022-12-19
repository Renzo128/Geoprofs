using Geoprofs.Models.Data;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: Password,
                salt: System.Text.Encoding.UTF8.GetBytes("string"),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
            var data = _context.logins
                .Where(l => l.Username == Username && l.Password == hashed);
            if (data.Any())
            {
                var otherData = _context.logins.Where(x => x.Password == hashed && x.Username == Username).FirstOrDefault();

                var userdata = _context.coworkers.Where(x => x.coworkerId == otherData.Coworker).FirstOrDefault();

                TempData["userId"] = otherData.Coworker;
                TempData["username"] = Username;
                TempData["password"] = Password;
                TempData["supervisor"] = userdata.supervisor;
                TempData["role"] = userdata.position;
                if ((int)TempData["role"] >= 6)
                {
                    var supervising = _context.supervisors.Where(x => x.Coworker == userdata.coworkerId).FirstOrDefault();
                    TempData["isSupervisor"] = supervising.supervisorId;
                    TempData["supervisor"] = supervising.supervisorId;

                    var supervisor = (int)TempData["isSupervisor"];
                    TempData.Keep("isSupervisor");
                    var requests = _context.absenceRequests.Where(x => x.absenceStatus == "Openstaand" && x.coworker.supervisor == supervisor).Count();

                    TempData["requests"] = requests;

                }
                TempData.Keep("role");

                // aantal verlof dagen bij elkaar optellen
                var users = _context.absenceRequests.Where(x => x.coworker == userdata);
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
