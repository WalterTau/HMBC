using HBMC.Domain.Api.Services.Interface;
using HBMC.Domain.Api.Services.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace HBMC.Domain.Api
{
	public static class ServiceRegistrationExtensions
	{
	
        public static void RegisterDomainAPIServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddTransient<IBoatsService, ManageBoatService>();
            services.AddTransient<IShipsService, ManageShipsService>();
            services.AddTransient<IScheduleService, ManageScheduleService>();
            services.AddTransient<IHarbour , ManageHarborService>();
           
        }
    }
}
