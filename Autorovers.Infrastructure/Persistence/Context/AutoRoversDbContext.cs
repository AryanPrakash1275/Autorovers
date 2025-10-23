using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Autorovers.Application.Abstractions.Data;
using Autorovers.Common;
using Autorovers.Domain.Common; // AggregateRoot
using DomainUsers = Autorovers.Domain.Users;
using DomainTodos = Autorovers.Domain.Todos;
using DomainEntities = Autorovers.Domain.Entities;

namespace Autorovers.Infrastructure.Persistence.Context
{
    public sealed class AutoroversDbContext : DbContext, IApplicationDbContext
    {
        public AutoroversDbContext(DbContextOptions<AutoroversDbContext> options)
            : base(options) { }

        public DbSet<DomainUsers.User> Users => Set<DomainUsers.User>();
        public DbSet<DomainTodos.TodoItem> TodoItems => Set<DomainTodos.TodoItem>();
        public DbSet<DomainEntities.Vehicle> Vehicles => Set<DomainEntities.Vehicle>();
        public DbSet<DomainEntities.Location> Locations => Set<DomainEntities.Location>();
        public DbSet<DomainEntities.VehicleDetails> VehicleDetails => Set<DomainEntities.VehicleDetails>();
        public DbSet<DomainEntities.UserDetails> UserDetails => Set<DomainEntities.UserDetails>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schemas.Default);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoroversDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Finds aggregates that have pending domain events.
        /// Your DomainEventsDispatcher will read from this.
        /// </summary>
        public IEnumerable<AggregateRoot> GetAggregatesWithEvents() =>
            ChangeTracker.Entries<AggregateRoot>()
                         .Where(e => e.Entity.HasDomainEvents)
                         .Select(e => e.Entity)
                         .ToList();
    }
}
