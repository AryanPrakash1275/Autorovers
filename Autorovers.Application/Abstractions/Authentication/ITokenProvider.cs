using Autorovers.Domain.Users;

namespace Autorovers.Application.Abstractions.Authentication;

public interface ITokenProvider
{
    string Create(User user);
}
