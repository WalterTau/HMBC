using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace HBMC.Domain.Api.SharePoint.Services
{

    public class SharePointServiceConfig
    {
        
        public string ConnectionSharePointUrl { get; set; }
                                                                            

        public string Username { get; set; }

        public string Pasword { get; set; }
    }

    
}
