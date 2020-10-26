using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HBMC.Domain.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService<WeatherRootObject> _weatherService;
        private readonly ILogger<WeatherRootObject> _logger;

        public WeatherController(IWeatherService<WeatherRootObject> weatherService, ILogger<WeatherRootObject> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherRootObject>> Get()
        {
            return await _weatherService.GetWeather();
        }
       
    }
}
