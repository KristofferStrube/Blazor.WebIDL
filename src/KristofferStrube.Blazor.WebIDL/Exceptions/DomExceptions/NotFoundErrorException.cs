namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The object can not be found here.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#notfounderror">See the WebIDL definition here</see></remarks>
public class NotFoundErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public NotFoundErrorException(string message, Exception innerException) : base(message, NotFoundError, innerException) { }
}
