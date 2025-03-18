using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Application
{
    public static class DependencyInjection
    {
        // Extension method to add services to the container (kiterjesztési módszer)
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //services.AddMediatR(configuration =>
            //{
            //    configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            //});

            return services;
        }
    }
}
