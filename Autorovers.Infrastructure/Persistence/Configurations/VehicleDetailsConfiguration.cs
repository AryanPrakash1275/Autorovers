using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Autorovers.Infrastructure.Persistence.Configurations
{
    public class VehicleDetailsConfiguration : IEntityTypeConfiguration<VehicleDetails>
    {
        public void Configure(EntityTypeBuilder<VehicleDetails> builder)
        {
            builder.ToTable("VehicleDetails");
            builder.HasKey(u => u.VehicleId);
        }
    }
}
