using HBMC.Domain.Api.Models;
using HBMC.Domain.Api.Services.Interface;
using Microsoft.SharePoint.Client;
using SharePointHelper.CRUD;
using SharePointHelper.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Services.Service
{
    public class ManageBoatService : IBoatsService
    {

        private ISharePointServiceConfig _sharePointServiceConfig;
        public ManageBoatService(ISharePointServiceConfig sharePointServiceConfig )
        {
            _sharePointServiceConfig = sharePointServiceConfig;
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
            /// Define client context object
            /// Get all boats from SharePoint List using Library HBMC.Domain.Api.SharePoint.Services
            /// 
            ///</summary>
            var siteUrl = "https://harbourmbc.sharepoint.com";


            ClientContext clientContext = new ClientContext(_sharePointServiceConfig.ConnectionSharePointUrl);

            var boats = new List<Boats>();
            var getLists =   SharePointHelper.CRUD.CRUDOperations.GetAnyListData(clientContext, nameof(Boats));
            return boats;

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
