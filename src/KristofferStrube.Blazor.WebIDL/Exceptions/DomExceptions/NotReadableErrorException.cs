namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The I/O read operation failed.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#notreadableerror">See the WebIDL definition here</see></remarks>
public class NotReadableErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public NotReadableErrorException(string message, Exception innerException) : base(message, NotReadableError, innerException) { }
}
