using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autorovers.Application.Abstractions.DomainEvents;

public interface IDomainEventsDispatcher
{
    Task DispatchEventsAsync(CancellationToken CancellationToken = default);
}
