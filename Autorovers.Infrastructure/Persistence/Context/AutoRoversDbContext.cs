using Microsoft.EntityFrameworkCore;
using Autorovers.Domain.Entities;
using Autorovers.Infrastructure.Persistence.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Infrastructure.Persistence.Context
{
    public class AutoRoversDbContext: DbContext
    {
        public AutoRoversDbContext(DbContextOptions<AutoRoversDbContext> options)
         : base(options)
        {
        }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<VehicleDetails> VehicleDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoRoversDbContext).Assembly);
        }
    }
}
