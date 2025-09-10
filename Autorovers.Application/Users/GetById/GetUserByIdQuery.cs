using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
