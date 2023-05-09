﻿namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// An exception that encapsulates a name and an optional integer code, for compatibility with historically defined exceptions in the DOM.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-DOMException">See the WebIDL definition here</see></remarks>
public class DOMException : WebIDLException
{
    internal const string HierarchyRequestError = "HierarchyRequestError";
    internal const string WrongDocumentError = "WrongDocumentError";
    internal const string InvalidCharacterError = "InvalidCharacterError";
    internal const string NoModificationAllowedError = "NoModificationAllowedError";
    internal const string NotFoundError = "NotFoundError";
    internal const string NotSupportedError = "NotSupportedError";
    internal const string InUseAttributeError = "InUseAttributeError";
    internal const string InvalidStateError = "InvalidStateError";
    internal const string SyntaxError = "SyntaxError";
    internal const string InvalidModificationError = "InvalidModificationError";
    internal const string NamespaceError = "NamespaceError";
    internal const string SecurityError = "SecurityError";
    internal const string NetworkError = "NetworkError";
    internal const string AbortError = "AbortError";
    internal const string QuotaExceededError = "QuotaExceededError";
    internal const string TimeoutError = "TimeoutError";
    internal const string InvalidNodeTypeError = "InvalidNodeTypeError";
    internal const string DataCloneError = "DataCloneError";
    internal const string EncodingError = "EncodingError";
    internal const string NotReadableError = "NotReadableError";
    internal const string UnknownError = "UnknownError";
    internal const string ConstraintError = "ConstraintError";
    internal const string DataError = "DataError";
    internal const string TransactionInactiveError = "TransactionInactiveError";
    internal const string ReadOnlyError = "ReadOnlyError";
    internal const string VersionError = "VersionError";
    internal const string OperationError = "OperationError";
    internal const string NotAllowedError = "NotAllowedError";
    internal const string OptOutError = "OptOutError";

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