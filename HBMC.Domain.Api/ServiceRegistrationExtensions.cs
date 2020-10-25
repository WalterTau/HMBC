using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Api.Services.Service;
using HBMC.Domain.Api.SharePoint.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace HBMC.Domain.Api
{
	public static class ServiceRegistrationExtensions
	{
	
        public static void RegisterDomainAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SharePointServiceConfig>(configuration.GetSection("SharePointOnlineConfig").GetSection("Url"));
          

            services.AddSingleton<ISharePointConnectionSetting>(sp =>
             new SharePointConnectionSetting(sp.GetRequiredService<IOptions<SharePointServiceConfig>>().Value, configuration)
            );

            services.AddTransient<IBoatsService, BoatService>();
            services.AddTransient<IShipsService, ShipsService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IHarbour, HarborService>();
           
        }
    }
}
