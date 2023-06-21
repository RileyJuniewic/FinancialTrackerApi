namespace FinancialTracker.Common.Exceptions;

public static partial class Exceptions
{
    public class SavingsAccountException : Exception
    {
        public SavingsAccountException()
        {
        }

        public SavingsAccountException(string message)
            : base(message)
        {
        }

        public SavingsAccountException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}