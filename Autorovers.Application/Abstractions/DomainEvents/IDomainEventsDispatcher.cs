using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Application.Abstractions.DomainEvents;

public interface IDomainEvent { }

public interface IDomainEventsDispatcher
{
    Task DispatchAsync(CancellationToken ct = default);
}

