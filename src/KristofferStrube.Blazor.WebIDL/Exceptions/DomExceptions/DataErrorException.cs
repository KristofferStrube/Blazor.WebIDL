namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// Provided data is inadequate.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#dataerror">See the WebIDL definition here</see></remarks>
public class DataErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public DataErrorException(string message, Exception innerException) : base(message, DataError, innerException) { }
}
