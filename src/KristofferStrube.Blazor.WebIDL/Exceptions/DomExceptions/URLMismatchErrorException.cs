namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// There is no fitting summary as this error has been deprecated. It is still used in some APIs though.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#urlmismatcherror">See the WebIDL definition here</see></remarks>
public class URLMismatchErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public URLMismatchErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, URLMismatchError, jSStackTrace, innerException) { }
}
