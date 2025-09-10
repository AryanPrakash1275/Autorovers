using Microsoft.EntityFrameworkCore;
using Autorovers.Application.Abstractions.Data;

using DomainUsers = Autorovers.Domain.Users;
using DomainTodos = Autorovers.Domain.Todos;
using DomainEntities = Autorovers.Domain.Entities;

namespace Autorovers.Infrastructure.Persistence.Context
{
    public class AutoRoversDbContext : DbContext, IApplicationDbContext
    {
        public AutoRoversDbContext(DbContextOptions<AutoRoversDbContext> options) : base(options) { }
        public DbSet<DomainUsers.User> Users => Set<DomainUsers.User>();
        public DbSet<DomainTodos.TodoItem> TodoItems => Set<DomainTodos.TodoItem>();
        public DbSet<DomainEntities.Vehicle> Vehicles => Set<DomainEntities.Vehicle>();
        public DbSet<DomainEntities.Location> Locations => Set<DomainEntities.Location>();
        public DbSet<DomainEntities.VehicleDetails> VehicleDetails => Set<DomainEntities.VehicleDetails>();
        public DbSet<DomainEntities.UserDetails> UserDetails => Set<DomainEntities.UserDetails>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoRoversDbContext).Assembly);
        }
    }
}
