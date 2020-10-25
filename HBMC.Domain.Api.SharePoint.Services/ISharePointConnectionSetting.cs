using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBMC.Domain.Api
{
    public interface ISharePointConnectionSetting
    {
        string ConnectionSharePointUrl { get; }
        string Username { get; }
        string Pasword { get; }
    }
}
