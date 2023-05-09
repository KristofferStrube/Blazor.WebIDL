namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// A request was placed against a transaction which is currently not active, or which is finished.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#transactioninactiveerror">See the WebIDL definition here</see></remarks>
public class TransactionInactiveErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public TransactionInactiveErrorException(string message, Exception innerException) : base(message, TransactionInactiveError, innerException) { }
}
