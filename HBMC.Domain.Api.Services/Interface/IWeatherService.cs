using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherRootObject>> GetWeather();
    }
}
