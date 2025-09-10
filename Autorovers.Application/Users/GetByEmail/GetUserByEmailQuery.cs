using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Users.GetByEmail;

public sealed record GetUserByEmailQuery(string Email) : IQuery<UserResponse>;
