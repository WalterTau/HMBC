using HBMC.Domain.Api.Helper;
using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Weather.RestClientService
{
    public class WeatherRestDomainService 
    {
        private IConfiguration _configuration;
      
        public WeatherRestDomainService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<WeatherRootObject>> ConsumeWeatherApi()
        {
            var client = new RestClient<IEnumerable<WeatherRootObject>>(_configuration);
            string url = null;
            UrlHelper urlHelper = new UrlHelper(_configuration);

            var apiUrl = urlHelper.WeatherUrl(url);
            var result = await client.Get(apiUrl);
            return result;

        }
    }
}
