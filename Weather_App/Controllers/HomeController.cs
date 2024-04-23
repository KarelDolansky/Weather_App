using Microsoft.AspNetCore.Http;
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
        [BindProperty]
        public WeatherData WeatherData { get; set; }

        public HomeController(ILogger<HomeController> logger, IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Poèasí";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Weather(string location)
        {
            WeatherData weatherData = _weatherService.GetWeather(location);
            //HttpContext.Session.Set<WeatherData>("UserWeatherData", weatherData);
            return View();
        }

        public IActionResult WeatherMore()
        {
            ViewData["Title"] = "Poèasí";
            Sesi["weatherData"] = WeatherData;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
