using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class ScheduleService : IScheduleService
    {
        public async Task<Schedule> Add(Schedule model)
        {
            throw new NotImplementedException();
        }

        public async Task<Schedule> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedules()
        {
            throw new NotImplementedException();
        }

        public async Task<Schedule> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Schedule> Update()
        {
            throw new NotImplementedException();
        }
    }
}
