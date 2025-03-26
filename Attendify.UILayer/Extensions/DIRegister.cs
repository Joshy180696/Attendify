using Attendify.DataLayer.Classes;
using Attendify.DataLayer.Context;
using Attendify.DomainLayer.Classes;
using Attendify.DomainLayer.Interfaces;
using Attendify.UILayer.Classes;
using Attendify.UILayer.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Attendify.UILayer.Extensions
{
    public static class DIRegister
    {
        public static IServiceCollection AddPlicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Core Services
            services.AddControllersWithViews(opt =>
            {
                opt.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEventsRepository, SqlEventsRepository>();
            services.AddScoped<IEventService, EventsService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<IRSVPRepository, SqlRSVPRepository>();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("AttendifyDB"));
            });

            #endregion

            #region Logging
            services.AddLogging(log =>
            {
                log.AddConsole();
                log.AddDebug();
            });
            #endregion


            return services;
        
        }
    }
}
