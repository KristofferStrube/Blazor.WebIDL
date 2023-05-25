namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The attribute is in use by another element.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#inuseattributeerror">See the WebIDL definition here</see></remarks>
public class InUseAttributeErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public InUseAttributeErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, InUseAttributeError, jSStackTrace, innerException) { }
}
