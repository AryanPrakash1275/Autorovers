using System.Threading;
using System.Threading.Tasks;
using Autorovers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Autorovers.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Autorovers.Domain.Users.User> Users { get; }
        DbSet<Autorovers.Domain.Todos.TodoItem> TodoItems { get; }
        DbSet<Vehicle> Vehicles { get; }
        DbSet<VehicleDetails> VehicleDetails { get; }
        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
