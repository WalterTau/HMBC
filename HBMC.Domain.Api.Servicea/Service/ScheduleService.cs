using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Api.Services.SharePointOnlineServiceHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class ScheduleService : IScheduleService
    {
        private IConfiguration _configuration;
        private ISharePointServiceHelper _sharePointServiceHelper;
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
            var model = await _sharePointServiceHelper.GetScheduleSharePointList();
            return model;
        }

        public async Task<Schedule> GetById(string id)
        {
            var model = await _sharePointServiceHelper.GetScheduleSharePointList();
            return model.Where(i => i.Id == id).FirstOrDefault();
        }

        public async Task<Schedule> Update()
        {
            throw new NotImplementedException();
        }
    }
}
