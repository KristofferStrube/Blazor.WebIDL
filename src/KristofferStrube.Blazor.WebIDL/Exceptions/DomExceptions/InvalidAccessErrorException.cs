namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// There is no fitting summary as this error has been deprecated in favor of <see cref="TypeErrorException"/>, <see cref="NotSupportedErrorException"/>, or <see cref="NotAllowedErrorException"/>. It is still used in some APIs though.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#invalidaccesserror">See the WebIDL definition here</see></remarks>
public class InvalidAccessErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public InvalidAccessErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, InvalidAccessError, jSStackTrace, innerException) { }
}
