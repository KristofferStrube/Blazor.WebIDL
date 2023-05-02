namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The operation is not allowed by Namespaces in XML.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#namespaceerror">See the WebIDL definition here</see></remarks>
public class NamespaceErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public NamespaceErrorException(string message, Exception innerException) : base(message, "NamespaceError", innerException) { }
}
