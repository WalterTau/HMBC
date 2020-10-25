using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.SharePoint.Client;
using SharePointHelper.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class BoatService : IBoatsService
    {

        IConfiguration _configuration;
        public BoatService(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        public async Task<Boats> Add(Boats model)
        {
            throw new NotImplementedException();
        }

        public async Task<Boats> Delete()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Boats>> GetAllBoats()
        {
            ///<summary>
            /// Get all boats from SharePoint List using Library HBMC.Domain.Api.SharePoint.Services
            /// 
            ///</summary>
            var model = SharePointHelper.CRUD.CRUDOperations.GetAnyListData(nameof(Boats));
            return (IEnumerable<Boats>)model.ToList();
        }

        public async Task<Boats> GetById(string Id)
        {
            throw new NotImplementedException();
        }

        public  async Task<Boats> Update()
        {
            throw new NotImplementedException();
        }
    }
}
