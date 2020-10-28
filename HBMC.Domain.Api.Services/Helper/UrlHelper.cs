using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.Helper
{
    public class UrlHelper
    {
        IConfiguration configuration;
        public UrlHelper(IConfiguration configuration)
        {
            this.configuration = configuration;  
        }

        public  string WeatherUrl(string url)
        {
            var WeatherUrl = configuration.GetSection("WeatherApiUrl").Value;
            var ApiKey = configuration.GetSection("Apikey").Value;

            return url = string.Format("{0}{1}{2}{3}", WeatherUrl.ToString() , "Durban", "&appid=",
                                                       ApiKey.ToString()).ToString();
            
        }
    }
}
