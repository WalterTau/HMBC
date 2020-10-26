using HBMC.Domain.Api.Helper;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Weather.RestClientService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class WeatherService : IWeatherService
    {
        private IConfiguration _configuration;

        public WeatherService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<WeatherRootObject>> GetWeather()
        {
            string url = null;
            var urlHelper = new UrlHelper(_configuration);
            var apiUrl = urlHelper.WeatherUrl(url);


            var client = new RestClient<IEnumerable<WeatherRootObject>>(_configuration);
            var results = await client.Get(apiUrl);
            return (IEnumerable<WeatherRootObject>)results;

        }
    }
}
