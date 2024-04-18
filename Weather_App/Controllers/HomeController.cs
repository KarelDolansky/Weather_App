using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Weather_App.Models;
using Weather_App.Services;

namespace Weather_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWeatherService _weatherService;

        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        //[HttpPost]
        //public string Weather(string location)
        //{
        //    return _weatherService.GetWeather(location);
        //}

        [HttpPost]
        public IActionResult WeatherBox(string location)
        {
            // Mùžete provést jakékoliv další operace, jako je napøíklad získání dat o poèasí na základì zadané lokace.
            ViewData["Location"] = location;

            return PartialView("_WeatherBox");
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
