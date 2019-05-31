// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.Profile.Domain.Models.Profile;
using Softeq.NetKit.Profile.SQLRepository.Mappings;

namespace Softeq.NetKit.Profile.SQLRepository
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> UserProfile { get; set; }

        public override int SaveChanges()
        {
            this.AddTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            this.AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //put db configuration here
            base.OnModelCreating(builder);

            builder.AddEntityConfigurationsFromAssembly(GetType().Assembly);
        }

        private void AddTimestamps()
        {
            var entitiesAdded = this.ChangeTracker.Entries().Where(x => x.Entity is ICreated && x.State == EntityState.Added);
            foreach (var entity in entitiesAdded)
            {
                ((ICreated)entity.Entity).Created = DateTime.UtcNow;
            }

            var entitiesModified = this.ChangeTracker.Entries().Where(x => x.Entity is IUpdated && x.State == EntityState.Modified);
            foreach (var entity in entitiesModified)
            {
                ((IUpdated)entity.Entity).Updated = DateTime.UtcNow;
            }
        }
    }
}