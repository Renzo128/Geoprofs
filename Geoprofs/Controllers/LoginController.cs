using Microsoft.AspNetCore.Mvc;
using Geoprofs.Models;
using Geoprofs.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Geoprofs.Controllers
{
    public class LoginController : Controller
    {
        private readonly DB _context;

        public LoginController(DB context)  // database data ophalen
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Verify( string Password , string Username)
        {   // controlleren of gebruiker de goede gebruikersnaam en wachtwoord gebruikt
            var Data = _context.logins
                .Where(l => l.Username == Username && l.Password == Password);
            if (Data.Any())
            {
                return RedirectToAction("index", "Coworkers");
            }
            else
            {
                return Content("Failed");
            }

        }
    }
}
