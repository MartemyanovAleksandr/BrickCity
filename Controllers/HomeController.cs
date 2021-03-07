using BrickCity.Models;
using BrickCity.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BrickCity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BrickDB _db;
        public HomeController(ILogger<HomeController> logger,BrickDB context)
        {
            _db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }    
        public IActionResult GetHouseConsupmption()
        {
            var res = _db.HouseConsumption.ToList();
            return res.ToJson(); 
        }            
        public IActionResult GetPlantConsupmption()
        {
            var res = _db.PlantsConsumption.ToList();         
            return res.ToJson();
        }
        public IActionResult GetHouses()
        {
            var res = _db.House.ToList();         
            return res.ToJson();
        }
        public IActionResult GetPlants()
        {
            var res = _db.Plant.ToList();         
            return res.ToJson();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
