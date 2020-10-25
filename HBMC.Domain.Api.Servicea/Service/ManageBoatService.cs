using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class ManageBoatService : IBoatsService
    {
        public async Task<Boat> Add(Boat model)
        {
            throw new NotImplementedException();
        }

        public async Task<Boat> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Boat>> GetAllBoats()
        {
            throw new NotImplementedException();
        }

        public async Task<Boat> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public  async Task<Boat> Update()
        {
            throw new NotImplementedException();
        }
    }
}
