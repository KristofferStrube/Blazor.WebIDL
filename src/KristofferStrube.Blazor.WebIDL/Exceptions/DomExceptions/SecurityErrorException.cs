namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The operation is insecure.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#securityerror">See the WebIDL definition here</see></remarks>
public class SecurityErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public SecurityErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, SecurityError, jSStackTrace, innerException) { }
}
