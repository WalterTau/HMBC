using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HBMC.Domain.Api.Helper
{
    public static class RouteHelper
    {
        public const string Index = "";
        public const string Version = Index + "v{version:apiVersion}/";

        public static class V1
        {
            public const string Swagger = "v1/swagger.json";
            public const string Api = Version;
            public const string ApiNumber = "1";

            public const string ManageBoats = "api/" + Version + "boats/";
            public const string ManageShips = "api/" + Version + "ships/";
            public const string ManageHarbour = "api/" + Version + "harbour/";
            public const string ManageSchedule = "api/" + Version + "schedule/";
            public const string Weather = "api/" + Version + "weather/";
  

        }
    }
}
