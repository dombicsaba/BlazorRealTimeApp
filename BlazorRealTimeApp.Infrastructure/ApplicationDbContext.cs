using BlazorRealTimeApp.Application.Common.Interfaces;
using BlazorRealTimeApp.Domain.Articles;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IRealTimeNotifier _realTimeNotifier;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IRealTimeNotifier realTimeNotifier) 
            : base(options)
        {
            _realTimeNotifier = realTimeNotifier;
        }

        public DbSet<Article> Articles { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            if (changes.Any())
            {
                await _realTimeNotifier.NotifyArticlesUpdated();
            }

            return result;
        }
    }
}
