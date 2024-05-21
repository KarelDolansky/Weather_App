using Microsoft.AspNetCore.Http.HttpResults;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Weather(string location, float? latitude, float? longitude, int days, DateOnly date)
        {
            if(location == null)
            {
                TempData["ErrorMessage"] = "Špatnì zadané místo";
                return RedirectToAction("Error");
            }
            if (date.AddMonths(1) < DateOnly.FromDateTime(DateTime.Now) || date > DateOnly.FromDateTime(DateTime.Now).AddMonths(1))
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }

            if (days < -1 || days > 1)
            {
                days = 0;
            }
            date = date.AddDays(days);
            location = location.ToLower();
            try
            {
                var data = latitude.HasValue && longitude.HasValue ?
                _weatherService.GetWeather(location, latitude.Value, longitude.Value, date) :
                _weatherService.GetWeather(location, date);
                ViewData["weatherData"] = data;
                ViewData["Icon"] = _ikonWeather.GetIcon(data.daily.weather_code[0]);
            }
            catch (ExceptionBadRequest ex)
            {
                TempData["ErrorMessage"] = "Špatnì zadané místo";
                return RedirectToAction("Error");
            }
            catch(ExceptionApiCall ex)
            {
                TempData["ErrorMessage"] = "Chyba pøi získávání dat o poèasí. Zkuste to prosím znovu.";
                return RedirectToAction("Error");
            }            
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Nastala neoèekávána chyba. Zkuste to prosím znovu.";
                return RedirectToAction("Error");
            }

            ViewData["Days"] = days;
            ViewData["Date"] = date;
            ViewData["location"] = location;

            return View();
        }


        [HttpGet]
        public IActionResult WeatherMore(string location, float? latitude, float? longitude, DateOnly date)
        {
            if (location == null)
            {
                TempData["ErrorMessage"] = "Špatnì zadané místo";
                return RedirectToAction("Error");
            }
            if (date.AddMonths(1) < DateOnly.FromDateTime(DateTime.Now) || date > DateOnly.FromDateTime(DateTime.Now).AddMonths(1))
            {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            try
            {
            var data = latitude.HasValue && longitude.HasValue ?
                _weatherService.GetWeather(location, latitude.Value, longitude.Value, date) :
                _weatherService.GetWeather(location, date);
                ViewData["weatherData"] = data;
            }
            catch (ExceptionBadRequest ex)
            {
                TempData["ErrorMessage"] = "Špatnì zadané místo";
                return RedirectToAction("Error");
            }
            catch (ExceptionApiCall ex)
            {
                TempData["ErrorMessage"] = "Chyba pøi získávání dat o poèasí. Zkuste to prosím znovu.";
                return RedirectToAction("Error");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Nastala neoèekávána chyba. Zkuste to prosím znovu.";
                return RedirectToAction("Error");
            }
            
            ViewData["location"] = location;
            return View();
        }
        [HttpPost]
        public IActionResult Favorite()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            var favorites = _favoriteService.GetAll(User).Result;

            return PartialView("_Favorite", favorites);
        }

        [HttpGet]
        public IActionResult AddFavorite(string location, float latitude, float longitude)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            _favoriteService.Add(location, latitude, longitude, User).Wait();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult RemoveFavorite(string location)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }
            _favoriteService.Remove(location, User).Wait();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = TempData["ErrorMessage"]?.ToString()
            };
            return View(model);
        }
    }
}
