﻿using Geoprofs.Models;
using Geoprofs.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Geoprofs.Controllers
{
    public class HomeController : Controller
    {
        #region --db connectie en logger
        private readonly ILogger<HomeController> _logger;

        private readonly DB _context;



        public HomeController(ILogger<HomeController> logger, DB context)
        {
            _logger = logger;
            _context = context;
        }
        #endregion

        #region --start pagina en gebruiker uitloggen
        public IActionResult Index()
        {

            TempData["supervisor"] = null;
            TempData["user_id"] = null;
            TempData["role"] = null;
            TempData["pos"] = null;
            TempData["sup"] = null;
            TempData["cow"] = null;
            TempData["username"] = null;
            TempData["password"] = null;
            TempData["Requests"] = null;
            TempData["absenceDays"] = null;
            TempData["abs"] = null;
            TempData["Month"] = null;
            TempData["Year"] = null;

            return View();
        }
        #endregion

        #region --privacy pagina
        public IActionResult Privacy()
        {
            return View();
        }
        #endregion

        #region --gebruiker registeren
        public IActionResult Register()
        {
           var positions =  JsonConvert.SerializeObject(_context.positions.ToList());
            var supervisors = JsonConvert.SerializeObject(_context.supervisors.ToList());
            var Coworkers = JsonConvert.SerializeObject(_context.coworkers.ToList());


            TempData["pos"] = positions;
            TempData["sup"] = supervisors;
            TempData["cow"] = Coworkers;
            return View();
        }

        public async Task<ActionResult> RegisterUser(string Fname_reg, string Lname_reg, string bsn_reg, int positie_reg, int Superviser_reg, string Username_reg, string Password_reg)
        {
            Coworker newCoworker = new Coworker() { CoworkerName = Fname_reg, coworkerLastname = Lname_reg, bsn = bsn_reg, position = positie_reg, supervisor = Superviser_reg, absence = 2,vacationdays = 25  };

            _context.Add(newCoworker);
            await _context.SaveChangesAsync();
            var data = _context.coworkers.Where(x => x.CoworkerName == Fname_reg && x.coworkerLastname == Lname_reg).FirstOrDefault();
            Login account = new Login() {  Coworker = data.coworkerId, Password = Password_reg, Username = Username_reg };

            _context.Add(account);
            await _context.SaveChangesAsync();

            TempData["supervisor"] = Superviser_reg;
            TempData["user_id"] = data.coworkerId;
            TempData["role"] = data.position;

            if(positie_reg >= 5)
            {
                Supervisor sup = new Supervisor() { Coworker = data.coworkerId };
                _context.Add(sup);
                _context.SaveChanges();
            }


            return RedirectToAction("index", "Coworkers");



        }
        #endregion

        #region --error melding
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
