using HBMC.Domain.Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class WeatherRestDomainService<T> : IWeatherService<T>
        where T : class
    {
        public Task<IEnumerable<T>> GetWeather()
        {
            throw new NotImplementedException();
        }
    }
}
