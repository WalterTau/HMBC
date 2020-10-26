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
    public class HarborService : IHarbourService
    {
        private IConfiguration _configuration;
        private ISharePointServiceHelper _sharePointServiceHelper;

        public HarborService(IConfiguration configuration, ISharePointServiceHelper sharePointServiceHelper)
        {

            _configuration = configuration;
            _sharePointServiceHelper = sharePointServiceHelper;
        }
        public async Task<Harbor> Add(Harbor model)
        {
            throw new NotImplementedException();
        }

        public async Task<Harbor> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Harbor>> GetAllHarbour()
        {
            var model = await _sharePointServiceHelper.GetHarbourSharePointList();
            return model;

        }

        public async Task<Harbor> GetById(string id)
        {
            var model = await _sharePointServiceHelper.GetHarbourSharePointList();
            return model.Where(i => i.Id == id).FirstOrDefault();
        }

        public async Task<Harbor> Update()
        {
            throw new NotImplementedException();
        }
    }
}
