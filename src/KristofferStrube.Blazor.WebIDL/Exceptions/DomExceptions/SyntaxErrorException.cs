namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The string did not match the expected pattern.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#syntaxerror">See the WebIDL definition here</see></remarks>
public class SyntaxErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public SyntaxErrorException(string message, Exception innerException) : base(message, SyntaxError, innerException) { }
}
