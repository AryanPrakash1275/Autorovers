using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Autorovers.Application.Abstractions.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Autorovers.Domain.Users.User> Users { get; }
        DbSet<Autorovers.Domain.Todos.TodoItem> TodoItems { get; }

        DbSet<T> Set<T>() where T : class;
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
