using KristofferStrube.Blazor.WebIDL.Exceptions;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Standard error mappers for Error types.
/// </summary>
public static class ErrorMappers
{
    /// <summary>
    /// The default mapper is used when none other is specified. It is a dictionary that maps from error names to a creator method that takes the name, message, stack trace, and inner exception and creates a new <see cref="WebIDLException"/>.
    /// </summary>
    public static Dictionary<string, Func<string, string, string?, Exception, WebIDLException>> Default { get; } = new()
    {
        { DOMException.HierarchyRequestError, (name, message, jSStackTrace, innerException) => new HierarchyRequestErrorException(message, jSStackTrace, innerException) },
        { DOMException.WrongDocumentError, (name, message, jSStackTrace, innerException) => new WrongDocumentErrorException(message, jSStackTrace, innerException) },
        { DOMException.InvalidCharacterError, (name, message, jSStackTrace, innerException) => new InvalidCharacterErrorException(message, jSStackTrace, innerException) },
        { DOMException.NoModificationAllowedError, (name, message, jSStackTrace, innerException) => new NoModificationAllowedErrorException(message, jSStackTrace, innerException) },
        { DOMException.NotFoundError, (name, message, jSStackTrace, innerException) => new NotFoundErrorException(message, jSStackTrace, innerException) },
        { DOMException.NotSupportedError, (name, message, jSStackTrace, innerException) => new NotSupportedErrorException(message, jSStackTrace, innerException) },
        { DOMException.InUseAttributeError, (name, message, jSStackTrace, innerException) => new InUseAttributeErrorException(message, jSStackTrace, innerException) },
        { DOMException.InvalidStateError, (name, message, jSStackTrace, innerException) => new InvalidStateErrorException(message, jSStackTrace, innerException) },
        { DOMException.SyntaxError, (name, message, jSStackTrace, innerException) => new SyntaxErrorDOMException(message, jSStackTrace, innerException) },
        { DOMException.InvalidModificationError, (name, message, jSStackTrace, innerException) => new InvalidModificationErrorException(message, jSStackTrace, innerException) },
        { DOMException.NamespaceError, (name, message, jSStackTrace, innerException) => new NamespaceErrorException(message, jSStackTrace, innerException) },
        { DOMException.SecurityError, (name, message, jSStackTrace, innerException) => new SecurityErrorException(message, jSStackTrace, innerException) },
        { DOMException.NetworkError, (name, message, jSStackTrace, innerException) => new NetworkErrorException(message, jSStackTrace, innerException) },
        { DOMException.AbortError, (name, message, jSStackTrace, innerException) => new AbortErrorException(message, jSStackTrace, innerException) },
        { DOMException.QuotaExceededError, (name, message, jSStackTrace, innerException) => new QuotaExceededErrorException(message, jSStackTrace, innerException) },
        { DOMException.TimeoutError, (name, message, jSStackTrace, innerException) => new TimeoutErrorException(message, jSStackTrace, innerException) },
        { DOMException.InvalidNodeTypeError, (name, message, jSStackTrace, innerException) => new InvalidNodeTypeErrorException(message, jSStackTrace, innerException) },
        { DOMException.DataCloneError, (name, message, jSStackTrace, innerException) => new DataCloneErrorException(message, jSStackTrace, innerException) },
        { DOMException.EncodingError, (name, message, jSStackTrace, innerException) => new EncodingErrorException(message, jSStackTrace, innerException) },
        { DOMException.NotReadableError, (name, message, jSStackTrace, innerException) => new NotReadableErrorException(message, jSStackTrace, innerException) },
        { DOMException.UnknownError, (name, message, jSStackTrace, innerException) => new UnknownErrorException(message, jSStackTrace, innerException) },
        { DOMException.ConstraintError, (name, message, jSStackTrace, innerException) => new ConstraintErrorException(message, jSStackTrace, innerException) },
        { DOMException.DataError, (name, message, jSStackTrace, innerException) => new DataErrorException(message, jSStackTrace, innerException) },
        { DOMException.TransactionInactiveError, (name, message, jSStackTrace, innerException) => new TransactionInactiveErrorException(message, jSStackTrace, innerException) },
        { DOMException.ReadOnlyError, (name, message, jSStackTrace, innerException) => new ReadOnlyErrorException(message, jSStackTrace, innerException) },
        { DOMException.VersionError, (name, message, jSStackTrace, innerException) => new VersionErrorException(message, jSStackTrace, innerException) },
        { DOMException.OperationError, (name, message, jSStackTrace, innerException) => new OperationErrorException(message, jSStackTrace, innerException) },
        { DOMException.NotAllowedError, (name, message, jSStackTrace, innerException) => new NotAllowedErrorException(message, jSStackTrace, innerException) },
        { DOMException.OptOutError, (name, message, jSStackTrace, innerException) => new OptOutErrorException(message, jSStackTrace, innerException) },
        { "EvalError", (name, message, jSStackTrace, innerException) => new EvalErrorException(message, jSStackTrace, innerException) },
        { "RangeError", (name, message, jSStackTrace, innerException) => new RangeErrorException(message, jSStackTrace, innerException) },
        { "ReferenceError", (name, message, jSStackTrace, innerException) => new ReferenceErrorException(message, jSStackTrace, innerException) },
        { "SyntaxError", (name, message, jSStackTrace, innerException) => new TypeErrorException(message, jSStackTrace, innerException) },
        { "TypeError", (name, message, jSStackTrace, innerException) => new TypeErrorException(message, jSStackTrace, innerException) },
        { "URIError", (name, message, jSStackTrace, innerException) => new URIErrorException(message, jSStackTrace, innerException) },
    };
}
