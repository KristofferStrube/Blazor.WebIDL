namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// An exception that encapsulates a name for an exception.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-DOMException">See the WebIDL definition here</see></remarks>
public class DOMException : WebIDLException
{
    public const string HierarchyRequestError = "HierarchyRequestError";
    public const string WrongDocumentError = "WrongDocumentError";
    public const string InvalidCharacterError = "InvalidCharacterError";
    public const string NoModificationAllowedError = "NoModificationAllowedError";
    public const string NotFoundError = "NotFoundError";
    public const string NotSupportedError = "NotSupportedError";
    public const string InUseAttributeError = "InUseAttributeError";
    public const string InvalidStateError = "InvalidStateError";
    public const string SyntaxError = "DOMExceptionSyntaxError";
    public const string InvalidModificationError = "InvalidModificationError";
    public const string NamespaceError = "NamespaceError";
    public const string SecurityError = "SecurityError";
    public const string NetworkError = "NetworkError";
    public const string AbortError = "AbortError";
    public const string QuotaExceededError = "QuotaExceededError";
    public const string TimeoutError = "TimeoutError";
    public const string InvalidNodeTypeError = "InvalidNodeTypeError";
    public const string DataCloneError = "DataCloneError";
    public const string EncodingError = "EncodingError";
    public const string NotReadableError = "NotReadableError";
    public const string UnknownError = "UnknownError";
    public const string ConstraintError = "ConstraintError";
    public const string DataError = "DataError";
    public const string TransactionInactiveError = "TransactionInactiveError";
    public const string ReadOnlyError = "ReadOnlyError";
    public const string VersionError = "VersionError";
    public const string OperationError = "OperationError";
    public const string NotAllowedError = "NotAllowedError";
    public const string OptOutError = "OptOutError";

    /// <summary>
    /// Error name which should be one of the ones listed in this <see href="https://webidl.spec.whatwg.org/#dfn-error-names-table">error names table</see>.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Constructs a wrapper Exception for the given error.
    /// </summary>
    /// <param name="message">User agent-defined value that provides human readable details of the error.</param>
    /// <param name="name">Error name which should be one of the ones listed in this <see href="https://webidl.spec.whatwg.org/#dfn-error-names-table">error names table</see>.</param>
    /// <param name="jSStackTrace">The stack trace from JavaScript if there is any.</param>
    /// <param name="innerException">Inner exception which is the cause of this exception.</param>
    protected DOMException(string message, string name, string? jSStackTrace, Exception innerException) : base(message, jSStackTrace, innerException)
    {
        Name = name;
    }
}
