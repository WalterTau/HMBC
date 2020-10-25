using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Interface
{
    public interface IScheduleService
    {
        Task<Schedule> Add(Schedule model);
        Task<IEnumerable<Schedule>> GetAllSchedules();
        Task<Schedule> GetById(string id);
        Task<Schedule> Delete();
        Task<Schedule> Update();
    }
}
