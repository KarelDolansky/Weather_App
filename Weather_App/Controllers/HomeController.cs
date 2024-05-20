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
        private readonly IFavoriteService _favoriteService;
        private readonly IIconWeather _ikonWeather;

        public HomeController(ILogger<HomeController> logger,
            IWeatherService weatherService,
            IFavoriteService favoriteService,
            IIconWeather ikonWeather)
        {
            _logger = logger;
            _weatherService = weatherService;
            _favoriteService = favoriteService;
            _ikonWeather = ikonWeather;
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Poèasí";
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["Title"] = "Poèasí";
            return View();
        }

        [HttpGet]
        public IActionResult Weather(string location, float? latitude, float? longitude)
        {
            ViewData["Title"] = "Poèasí";
            
            location = location.ToLower();


            var data = latitude.HasValue && longitude.HasValue ?
                _weatherService.GetWeather(location, latitude.Value, longitude.Value) :
                _weatherService.GetWeather(location);

            ViewData["weatherData"] = data;
            ViewData["Icon"] = _ikonWeather.GetIcon(data.daily.weather_code[0]);
            ViewData["location"] = location;

            return View();
        }

        [HttpGet]
        public IActionResult WeatherMore(string location)
        {
            ViewData["Title"] = "Poèasí";
            ViewData["weatherData"] = _weatherService.GetWeather(location);
            ViewData["location"] = location;
            return View();
        }
        [HttpPost]
        public IActionResult Favorite()
        {
            ViewData["Title"] = "Poèasí";
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var favorites = _favoriteService.GetFavorites(User).Result;

            return PartialView("_Favorite", favorites);
        }

        [HttpGet]
        public IActionResult AddFavorite(string location, float latitude, float longitude)
        {
            ViewData["Title"] = "Poèasí";
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            _favoriteService.AddFavorite(location, latitude, longitude, User).Wait();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult RemoveFavorite(string location)
        {
            ViewData["Title"] = "Poèasí";
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            _favoriteService.RemoveFavorite(location, User).Wait();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                ?? HttpContext.TraceIdentifier
            });
        }
    }
}
