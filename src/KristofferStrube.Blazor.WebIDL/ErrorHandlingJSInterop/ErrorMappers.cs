using KristofferStrube.Blazor.WebIDL.Exceptions;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Standard Handlers for Error types.
/// </summary>
public static class ErrorMappers
{
    /// <summary>
    /// The default mapper used when none is specified. It is a dictionary that maps from Error names to a creator method that takes the name, message, and inner exception and creates a new WebIDLException.
    /// </summary>
    public static Dictionary<string, Func<string, string, Exception, WebIDLException>> Default { get; } = new()
    {
        { DOMException.HierarchyRequestError, (name, message, innerException) => new HierarchyRequestErrorException(message, innerException) },
        { DOMException.WrongDocumentError, (name, message, innerException) => new WrongDocumentErrorException(message, innerException) },
        { DOMException.InvalidCharacterError, (name, message, innerException) => new InvalidCharacterErrorException(message, innerException) },
        { DOMException.NoModificationAllowedError, (name, message, innerException) => new NoModificationAllowedErrorException(message, innerException) },
        { DOMException.NotFoundError, (name, message, innerException) => new NotFoundErrorException(message, innerException) },
        { DOMException.NotSupportedError, (name, message, innerException) => new NotSupportedErrorException(message, innerException) },
        { DOMException.InUseAttributeError, (name, message, innerException) => new InUseAttributeErrorException(message, innerException) },
        { DOMException.InvalidStateError, (name, message, innerException) => new InvalidStateErrorException(message, innerException) },
        { DOMException.SyntaxError, (name, message, innerException) => new SyntaxErrorException(message, innerException) },
        { DOMException.InvalidModificationError, (name, message, innerException) => new InvalidModificationErrorException(message, innerException) },
        { DOMException.NamespaceError, (name, message, innerException) => new NamespaceErrorException(message, innerException) },
        { DOMException.SecurityError, (name, message, innerException) => new SecurityErrorException(message, innerException) },
        { DOMException.NetworkError, (name, message, innerException) => new NetworkErrorException(message, innerException) },
        { DOMException.AbortError, (name, message, innerException) => new AbortErrorException(message, innerException) },
        { DOMException.QuotaExceededError, (name, message, innerException) => new QuotaExceededErrorException(message, innerException) },
        { DOMException.TimeoutError, (name, message, innerException) => new TimeoutErrorException(message, innerException) },
        { DOMException.InvalidNodeTypeError, (name, message, innerException) => new InvalidNodeTypeErrorException(message, innerException) },
        { DOMException.DataCloneError, (name, message, innerException) => new DataCloneErrorException(message, innerException) },
        { DOMException.EncodingError, (name, message, innerException) => new EncodingErrorException(message, innerException) },
        { DOMException.NotReadableError, (name, message, innerException) => new NotReadableErrorException(message, innerException) },
        { DOMException.UnknownError, (name, message, innerException) => new UnknownErrorException(message, innerException) },
        { DOMException.ConstraintError, (name, message, innerException) => new ConstraintErrorException(message, innerException) },
        { DOMException.DataError, (name, message, innerException) => new DataErrorException(message, innerException) },
        { DOMException.TransactionInactiveError, (name, message, innerException) => new TransactionInactiveErrorException(message, innerException) },
        { DOMException.ReadOnlyError, (name, message, innerException) => new ReadOnlyErrorException(message, innerException) },
        { DOMException.VersionError, (name, message, innerException) => new VersionErrorException(message, innerException) },
        { DOMException.OperationError, (name, message, innerException) => new OperationErrorException(message, innerException) },
        { DOMException.NotAllowedError, (name, message, innerException) => new NotAllowedErrorException(message, innerException) },
        { DOMException.OptOutError, (name, message, innerException) => new OptOutErrorException(message, innerException) },
        { "EvalError", (name, message, innerException) => new EvalErrorException(message, innerException) },
        { "RangeError", (name, message, innerException) => new RangeErrorException(message, innerException) },
        { "ReferenceError", (name, message, innerException) => new ReferenceErrorException(message, innerException) },
        { "TypeError", (name, message, innerException) => new TypeErrorException(message, innerException) },
        { "URIError", (name, message, innerException) => new URIErrorException(message, innerException) },
    };
}
