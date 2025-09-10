using Autorovers.Application.Abstractions.Messaging;

namespace Autorovers.Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<string>;
