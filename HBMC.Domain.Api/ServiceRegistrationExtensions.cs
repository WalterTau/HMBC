using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Api.Services.Service;
using HBMC.Domain.Api.Services.SharePointOnlineServiceHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace HBMC.Domain.Api
{
	public static class ServiceRegistrationExtensions
	{
	
        public static void RegisterDomainAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SharePointServiceHelper>(configuration.GetSection("SharePointOnlineConfig").GetSection("Url"));
          

            services.AddSingleton<ISharePointServiceHelper>(sp =>
             new SharePointServiceConfig(sp.GetRequiredService<IOptions<SharePointServiceHelper>>().Value)
            );

            services.AddTransient<IBoatsService, BoatService>();
            services.AddTransient<IShipsService, ShipsService>();
            services.AddTransient<IScheduleService, ScheduleService>();
            services.AddTransient<IHarbourService, HarborService>();
           
        }
    }
}
