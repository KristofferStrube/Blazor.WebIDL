namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The request is not allowed by the user agent or the platform in the current context, possibly because the user denied permission.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#notallowederror">See the WebIDL definition here</see></remarks>
public class NotAllowedErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public NotAllowedErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, NotAllowedError, jSStackTrace, innerException) { }
}
