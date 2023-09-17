namespace FinancialTracker.Common.Exceptions;

public class ClaimNotFoundException : Exception
{
    public ClaimNotFoundException()
    {
        LogError();
    }

    public ClaimNotFoundException(string message)
        : base(message)
    {
        LogError();
    }

    public ClaimNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
        LogError();
    }

    private void LogError()
    {
        //throw new NotImplementedException();
    }
}