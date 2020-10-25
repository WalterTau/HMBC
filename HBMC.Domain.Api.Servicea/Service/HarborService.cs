using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class HarborService : IHarbour
    {
        public async Task<Harbor> Add(Harbor model)
        {
            throw new NotImplementedException();
        }

        public async Task<Harbor> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Boats>> GetAllHarbour()
        {
            throw new NotImplementedException();
        }

        public async Task<Harbor> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Harbor> Update()
        {
            throw new NotImplementedException();
        }
    }
}
