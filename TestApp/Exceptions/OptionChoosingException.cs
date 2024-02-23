namespace TestApp.Exceptions;

public class OptionChoosingException : Exception
{
    public string ErrorCode { get; }
    public OptionChoosingException(string message, string errorCode, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}