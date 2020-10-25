using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.SharePoint.Services
{
    public class SharePointConnectionSetting : ISharePointConnectionSetting
    {
        private SharePointServiceConfig _sharePointServiceConfig;
        private IConfiguration _configuration;
        public SharePointConnectionSetting(SharePointServiceConfig sharePointServiceConfig , IConfiguration configuration)
        {
            _sharePointServiceConfig = sharePointServiceConfig;
            _configuration = configuration;
        }

        public string ConnectionSharePointUrl { get => _configuration.GetSection("SharePointOnlineConfig").GetSection("Url").Value; }

        public string Username { get => _configuration.GetSection("SharePointOnlineConfig")
                                                            .GetSection("Login")
                                                               .GetSection("Üsername").Value; }

        public string Pasword { get => _configuration.GetSection("SharePointOnlineConfig")
                                                            .GetSection("Login")
                                                               .GetSection("Password").Value;
        }
    }
}
