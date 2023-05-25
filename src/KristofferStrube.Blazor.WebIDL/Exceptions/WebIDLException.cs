namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// A common exception class for all exceptions from the <c>Blazor.WebIDL</c> library.
/// </summary>
public class WebIDLException : Exception
{
    private readonly string? jSStackTrace;

    /// <summary>
    /// Returns the stack trace as a string. If no stack trace is available, null is returned. The stack is prepended with the JS stack trace if there is any.
    /// </summary>
    public override string? StackTrace => jSStackTrace is not null ? jSStackTrace + '\n' + base.StackTrace : base.StackTrace;

    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public WebIDLException(string message, string? jSStackTrace, Exception innerException) : base(message, innerException)
    {
        this.jSStackTrace = jSStackTrace;
    }
}
