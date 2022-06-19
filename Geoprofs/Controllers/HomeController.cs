using Geoprofs.Models;
using Geoprofs.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Geoprofs.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly DB _context;



        public HomeController(ILogger<HomeController> logger, DB context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Register()
        {
           var positions =  _context.positions;
            var supervisors = _context.supervisors;
            var Coworkers = _context.coworkers;



            TempData["sup"] = supervisors;
            TempData["cow"] = Coworkers;
            return View();
        }

        public async Task<ActionResult> RegisterUser(string Fname_reg, string Lname_reg, string bsn_reg, int positie_reg, int Superviser_reg, string Username_reg, string Password_reg)
        {
            int bsnconvert = Convert.ToInt32(bsn_reg);
            Coworker newCoworker = new Coworker() { CoworkerName = Fname_reg, coworkerLastname = Lname_reg, bsn = bsnconvert, position = positie_reg, supervisor = Superviser_reg, absence = 2,vacationdays = 25  };

            _context.Add(newCoworker);
            await _context.SaveChangesAsync();
            var data = _context.coworkers.Where(x => x.CoworkerName == Fname_reg && x.coworkerLastname == Fname_reg).FirstOrDefault();
            Login account = new Login() {  Coworker = data.coworkerId, Password = Password_reg, Username = Username_reg };

            _context.Add(account);
            await _context.SaveChangesAsync();



            return RedirectToAction("index", "Coworkers");



        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
