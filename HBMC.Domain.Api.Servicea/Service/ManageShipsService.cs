using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class ManageShipsService : IShipsService
    {
        public async Task<Ship> Add(Ship model)
        {
            throw new NotImplementedException();
        }

        public async Task<Ship> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ship>> GetAllBoats()
        {
            throw new NotImplementedException();
        }

        public async Task<Ship> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ship> Update()
        {
            throw new NotImplementedException();
        }
    }
}
