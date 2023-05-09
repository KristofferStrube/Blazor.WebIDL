namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The operation failed for an unknown transient reason (e.g. out of memory).
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#unknownerror">See the WebIDL definition here</see></remarks>
public class UnknownErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public UnknownErrorException(string message, Exception innerException) : base(message, UnknownError, innerException) { }
}
