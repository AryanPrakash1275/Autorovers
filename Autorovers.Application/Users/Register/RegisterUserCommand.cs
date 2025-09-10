using System;
using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Users.Register;

public sealed record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password
) : ICommand<Guid>;
