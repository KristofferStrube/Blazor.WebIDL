namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// An exception that encapsulates a name and an optional integer code, for compatibility with historically defined exceptions in the DOM.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-DOMException">See the WebIDL definition here</see></remarks>
public class DOMException : WebIDLException
{
    /// <summary>
    /// Error name which should be one of the ones listed in this <see href="https://webidl.spec.whatwg.org/#dfn-error-names-table">error names table</see>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="name">Error name which should be one of the ones listed in this <see href="https://webidl.spec.whatwg.org/#dfn-error-names-table">error names table</see>.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    protected DOMException(string message, string name, Exception innerException) : base(message, innerException)
    {
        Name = name;
    }
}
