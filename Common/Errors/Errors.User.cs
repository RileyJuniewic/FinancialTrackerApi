using ErrorOr;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class UserError
        {
            public static Error InvalidCredentials => Error.Failure(code: "User.InvalidCredentials", description: "Login failed due to invalid credentials.");
            public static Error UserNotFound => Error.NotFound(code: "User.UserNotFound", description: "User was not found in search.");
            public static Error DuplicateEmail => Error.Conflict(code: "User.DuplicateEmail", description: "Email is already registered.");
        }
    }
}
