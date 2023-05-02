namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// The operation would yield an incorrect node tree.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#hierarchyrequesterror">See the WebIDL definition here</see></remarks>
public class HierarchyRequestErrorException : DOMException
{
    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    public HierarchyRequestErrorException(string message, Exception innerException) : base(message, "HierarchyRequestError", innerException) { }
}
