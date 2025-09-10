using System;
using System.Threading;
using System.Threading.Tasks;
using Autorovers.Application.Abstractions.Authentication;
using Autorovers.Application.Abstractions.Data;
using Autorovers.Application.Abstractions.Messaging;
using Autorovers.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Autorovers.Common;

namespace Autorovers.Application.Users.Register;

public sealed class RegisterUserCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        if (await context.Users.AnyAsync(u => u.Email == command.Email, ct))
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);

        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            PasswordHash = passwordHasher.Hash(command.Password)
        };

        user.Raise(new UserRegisteredDomainEvent(user.Id));

        context.Users.Add(user);
        await context.SaveChangesAsync(ct);

        return Result.Success(user.Id);
    }
}
