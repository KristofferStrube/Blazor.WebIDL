namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The object is in an invalid state.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#invalidstateerror">See the WebIDL definition here</see></remarks>
public class InvalidStateErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public InvalidStateErrorException(string message, Exception innerException) : base(message, InvalidStateError, innerException) { }
}
