namespace FinancialTracker.Common.Exceptions;

public class UserIdParsingException : Exception
{
    public UserIdParsingException()
    {
        LogError();
    }

    public UserIdParsingException(string message)
        : base(message)
    {
        LogError();
    }

    public UserIdParsingException(string message, Exception inner)
        : base(message, inner)
    {
        LogError();
    }

    private void LogError()
    {
        //throw new NotImplementedException();
    }
}