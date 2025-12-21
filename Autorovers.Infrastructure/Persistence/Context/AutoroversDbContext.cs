using System.Collections.Generic;
using System.Linq;
using Autorovers.Application.Abstractions.Data;
using Autorovers.Domain.Common; // AggregateRoot
using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DomainEntities = Autorovers.Domain.Entities;
using DomainTodos = Autorovers.Domain.Todos;
using DomainUsers = Autorovers.Domain.Users;

namespace Autorovers.Infrastructure.Persistence.Context
{
    public sealed class AutoroversDbContext : DbContext, IApplicationDbContext
    {
        public AutoroversDbContext(DbContextOptions<AutoroversDbContext> options)
            : base(options) { }

        public DbSet<DomainUsers.User> Users => Set<DomainUsers.User>();
        public DbSet<DomainTodos.TodoItem> TodoItems => Set<DomainTodos.TodoItem>();

        public DbSet<DomainEntities.Vehicle> Vehicles => Set<DomainEntities.Vehicle>();
        public DbSet<DomainEntities.VehicleDetails> VehicleDetails => Set<DomainEntities.VehicleDetails>();

        public DbSet<VehicleVariant> VehicleVariants => Set<VehicleVariant>();
        public DbSet<VariantAddon> VariantAddons => Set<VariantAddon>();
        public DbSet<VehicleVariantAddon> VehicleVariantAddons => Set<VehicleVariantAddon>();

        // You still have these in your context; leaving them untouched:
        public DbSet<DomainEntities.Location> Locations => Set<DomainEntities.Location>();
        public DbSet<DomainEntities.UserDetails> UserDetails => Set<DomainEntities.UserDetails>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoroversDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public IEnumerable<AggregateRoot> GetAggregatesWithEvents() =>
            ChangeTracker.Entries<AggregateRoot>()
                         .Where(e => e.Entity.HasDomainEvents)
                         .Select(e => e.Entity)
                         .ToList();
    }
}
