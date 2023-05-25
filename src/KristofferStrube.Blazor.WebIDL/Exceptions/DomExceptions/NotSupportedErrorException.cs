namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The operation is not supported.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#notsupportederror">See the WebIDL definition here</see></remarks>
public class NotSupportedErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public NotSupportedErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, NotSupportedError, jSStackTrace, innerException) { }
}
