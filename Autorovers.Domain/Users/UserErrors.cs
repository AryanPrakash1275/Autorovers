using Autorovers.Common;

namespace Autorovers.Domain.Users;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "User not found.", ErrorType.NotFound);
    public static readonly Error DuplicateEmail = new("User.DuplicateEmail", "Email already in use.", ErrorType.Conflict);
    public static readonly Error Unauthorized = new("User.Unauthorized", "User is not authorized.", ErrorType.Unauthorized);
    public static readonly Error NotFoundByEmail = new("User.NotFoundByEmail", "User Email Not Found", ErrorType.NotFound);
    public static readonly Error EmailNotUnique = new("User.EmailNotUnique", "User Email Not Unique", ErrorType.Conflict);

    // OPTIONAL convenience methods if your code insists on calling them like functions:
    public static Error NotFoundError() => NotFound;
    public static Error DuplicateEmailError() => DuplicateEmail;
    public static Error UnauthorizedError() => Unauthorized;
}
