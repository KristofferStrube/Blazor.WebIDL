namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// A mutation operation in a transaction failed because a constraint was not satisfied.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#constrainterror">See the WebIDL definition here</see></remarks>
public class ConstraintErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public ConstraintErrorException(string message, Exception innerException) : base(message, "ConstraintError", innerException) { }
}
