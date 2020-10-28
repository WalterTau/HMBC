using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.SharePointOnlineServiceHelper
{
    public interface ISharePointServiceHelper
    {
        Task<IEnumerable<Boats>> GetBoatsSharePointList();
        Task<IEnumerable<Ships>> GetShipssSharePointList();
        Task<IEnumerable<Harbor>> GetHarbourSharePointList();
        Task<IEnumerable<Schedule>> GetScheduleSharePointList();
    }
}
