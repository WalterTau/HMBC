using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.SharePointOnlineServiceHelper
{
    public class SharePointServiceConfig : ISharePointServiceHelper
    {
        private SharePointServiceHelper _serviceHelper;

        public SharePointServiceConfig(SharePointServiceHelper serviceHelper)
        {
            _serviceHelper = serviceHelper;
        }

        public async Task<IEnumerable<Boats>> GetBoatsSharePointList()
        {
           
            var model = SharePointHelper.CRUD.CRUDOperations.GetAnyListData(nameof(Boats));
            return (IEnumerable<Boats>)model;
        }

        public async Task<IEnumerable<Harbor>> GetHarbourSharePointList()
        {
            var model = SharePointHelper.CRUD.CRUDOperations.GetAnyListData("Harbour");
            return (IEnumerable<Harbor>)model;
        }

        public async Task<IEnumerable<Schedule>> GetScheduleSharePointList()
        {
            var model = SharePointHelper.CRUD.CRUDOperations.GetAnyListData("Schedules");
            return (IEnumerable<Schedule>)model;
        }

        public async Task<IEnumerable<Ships>> GetShipssSharePointList()
        {
            var model = SharePointHelper.CRUD.CRUDOperations.GetAnyListData(nameof(Ships));
            return (IEnumerable<Ships>)model;
        }
    }
}
