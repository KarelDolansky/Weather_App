using Microsoft.AspNetCore.Mvc;
using Weather_App.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Weather_App.Controllers
{
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
    [Route("[controller]/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        public ApiController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet("{location}")]
        public string Get(string location)
        {
            return _weatherService.GetJson(location);
        }
    }
}
