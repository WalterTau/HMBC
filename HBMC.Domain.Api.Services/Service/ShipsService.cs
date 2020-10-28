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
    public class ShipsService : IShipsService
    {
        private IConfiguration _configuration;
        private ISharePointServiceHelper _sharePointServiceHelper;

        public ShipsService(IConfiguration configuration, ISharePointServiceHelper sharePointServiceHelper)
        {

            _configuration = configuration;
            _sharePointServiceHelper = sharePointServiceHelper;
        }

        public async Task<Ships> Add(Ships model)
        {
            throw new NotImplementedException();
        }

        public async Task<Ships> Delete()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ships>> GetAllBoats()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Ships>> GetAllShips()
        {
            var model = await _sharePointServiceHelper.GetShipssSharePointList();
            return model;
        }

        public async Task<Ships> GetById(string Id)
        {
            var model =await  _sharePointServiceHelper.GetShipssSharePointList();
            return model.Where(i => i.Id == Id).FirstOrDefault();
        }

        public async Task<Ships> Update(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
