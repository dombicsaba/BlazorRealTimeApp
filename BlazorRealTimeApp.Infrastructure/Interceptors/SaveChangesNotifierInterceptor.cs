using BlazorRealTimeApp.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure.Interceptors
{
    public class SaveChangesNotifierInterceptor : SaveChangesInterceptor
    {
        private readonly IServiceProvider _serviceProvider;

        public SaveChangesNotifierInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var notifier = scope.ServiceProvider.GetRequiredService<IRealTimeNotifier>();
                await notifier.NotifyArticlesUpdated("Article updated");
            }
            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }
}
