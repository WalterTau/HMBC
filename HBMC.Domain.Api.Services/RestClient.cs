using HBMC.Domain.Api.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Threading.Tasks;


namespace HBMC.Domain.Weather.RestClientService
{
    public class RestClient<T> where T : class
    {
        private IConfiguration _configuration;
      
        public RestClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //Handles Get Requests
        public async Task<T> Get(string url)
        {
          
            T result = null;

            using (var client = new HttpClient())
            {

                var response = client.GetAsync(new Uri(url)).Result;

                response.EnsureSuccessStatusCode();

                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                        throw x.Exception;
                    var results = "[" + x.Result + "]";
                    result = JsonConvert.DeserializeObject<T>(results);
                });
            }
            return result;
        }

       


    }


}
