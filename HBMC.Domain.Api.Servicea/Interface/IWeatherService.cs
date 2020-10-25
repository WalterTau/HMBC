using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IWeatherService<T> where T : class 
    {
        Task<IEnumerable<T>> GetWeather();
    }
}
