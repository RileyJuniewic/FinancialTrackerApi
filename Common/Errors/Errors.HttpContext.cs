using ErrorOr;

namespace FinancialTracker.Common.Errors
{
    public static partial class Errors
    {
        public static class HttpContextError
        {
            public static Error CannotFindClaimUserId => Error.Unexpected(code: "HttpContext.CannotFindClaimUserId", description: "HttpContext cannot find UserId in claims");
            public static Error HttpContextNull => Error.NotFound(code: "HttpContext.HttpContextNull", description: "HttpContext could not be located");
        }
    }
}
