using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Api.Services.SharePointOnlineServiceHelper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class BoatService : IBoatsService
    {
        private IConfiguration _configuration;
        private ISharePointServiceHelper _sharePointServiceHelper;

        public BoatService(IConfiguration configuration, ISharePointServiceHelper sharePointServiceHelper)
        {
          
            _configuration = configuration;
            _sharePointServiceHelper = sharePointServiceHelper;
        }

        public async Task<Boats> Add(Boats model)
        {
            // Todo
            throw new NotImplementedException();
        }

        public async Task<Boats> Delete()
        {
            // Todo
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Boats>> GetAllBoats()
        {
          
           var model = await _sharePointServiceHelper.GetBoatsSharePointList();
           return model;
           
        }

        public async Task<Boats> GetById(string Id)
        {
            var model = await _sharePointServiceHelper.GetBoatsSharePointList();
            return model.Where(i=>i.Id == Id).FirstOrDefault();
        }

        public  async Task<Boats> Update()
        {
            throw new NotImplementedException();
        }
    }
}
