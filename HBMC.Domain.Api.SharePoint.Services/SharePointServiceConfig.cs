using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.SharePoint.Services
{
    public class SharePointServiceConfig : ISharePointServiceConfig
    {
        private IConfiguration _configuration;

        public SharePointServiceConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionSharePointUrl => _configuration.GetSection("SharePointOnlineConfig")
                                                                    .GetSection("Url").Value;

        public string Username => _configuration.GetSection("SharePointOnlineConfig")
                                                          .GetSection("Login")
                                                             .GetSection("Username").Value;              

        public string Pasword => _configuration.GetSection("SharePointOnlineConfig")
                                                          .GetSection("Login")
                                                             .GetSection("Password").Value;
    }

    
}
