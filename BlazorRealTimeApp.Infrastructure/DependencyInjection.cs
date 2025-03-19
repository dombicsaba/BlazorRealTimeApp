using BlazorRealTimeApp.Application.Common.Interfaces;
using BlazorRealTimeApp.Domain.Articles;
using BlazorRealTimeApp.Infrastructure.Repositories;
using BlazorRealTimeApp.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure
{
    public static class DependencyInjection
    {// Extension method to add services to the container (kiterjesztési módszer)
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configure Serilog from appsettings.json
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // Add Serilog to the service collection
            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            services.AddDbContextFactory<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IArticleRepository, ArticleRepository>();

            // SignalR és az értesítő szolgáltatás regisztrálása
            services.AddSignalR();
            services.AddScoped<IRealTimeNotifier, SignalRNotifier>();

            return services;
        }
    }
}
