namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The user opted out of the process.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#optouterror">See the WebIDL definition here</see></remarks>
public class OptOutErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public OptOutErrorException(string message, Exception innerException) : base(message, OptOutError, innerException) { }
}
