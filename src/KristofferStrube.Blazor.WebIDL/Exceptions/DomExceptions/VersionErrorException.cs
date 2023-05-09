namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// An attempt was made to open a database using a lower version than the existing version.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#versionerror">See the WebIDL definition here</see></remarks>
public class VersionErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public VersionErrorException(string message, Exception innerException) : base(message, VersionError, innerException) { }
}
