namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The mutating operation was attempted in a "readonly" transaction.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#readonlyerror">See the WebIDL definition here</see></remarks>
public class ReadOnlyErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public ReadOnlyErrorException(string message, Exception innerException) : base(message, "ReadOnlyError", innerException) { }
}
