using HBMC.Domain.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using SharePointHelper.CRUD;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace HBMC.Domain.Api.Services.SharePointOnlineServiceHelper
{
    public class SharePointServiceHelper
    {

        public List<Boats> Boats { get; set; }
        public List<Ships> Ships { get; set; }


    }
}
