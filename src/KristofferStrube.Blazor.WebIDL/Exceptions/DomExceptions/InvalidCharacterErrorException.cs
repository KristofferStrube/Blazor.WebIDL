namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The string contains invalid characters.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#invalidcharactererror">See the WebIDL definition here</see></remarks>
public class InvalidCharacterErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public InvalidCharacterErrorException(string message, Exception innerException) : base(message, "InvalidCharacterError", innerException) { }
}
