using KristofferStrube.Blazor.WebIDL.Exceptions;
using System.Collections.ObjectModel;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Standard error mappers for Error types.
/// </summary>
public static class ErrorMappers
{
    /// <summary>
    /// The default mapper is used when none other is specified. It is a dictionary that maps from error names to a creator method that takes the name, message, stack trace, and inner exception and creates a new <see cref="WebIDLException"/>.
    /// </summary>
    public static ReadOnlyDictionary<string, Func<JSError, WebIDLException>> Default { get; } = new(new Dictionary<string, Func<JSError, WebIDLException>>()
    {
        { DOMException.HierarchyRequestError, (jSError) => new HierarchyRequestErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.WrongDocumentError, (jSError) => new WrongDocumentErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.InvalidCharacterError, (jSError) => new InvalidCharacterErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NoModificationAllowedError, (jSError) => new NoModificationAllowedErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NotFoundError, (jSError) => new NotFoundErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NotSupportedError, (jSError) => new NotSupportedErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.InUseAttributeError, (jSError) => new InUseAttributeErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.InvalidStateError, (jSError) => new InvalidStateErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.SyntaxError, (jSError) => new SyntaxErrorDOMException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.InvalidModificationError, (jSError) => new InvalidModificationErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NamespaceError, (jSError) => new NamespaceErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.SecurityError, (jSError) => new SecurityErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NetworkError, (jSError) => new NetworkErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.AbortError, (jSError) => new AbortErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.QuotaExceededError, (jSError) => new QuotaExceededErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.TimeoutError, (jSError) => new TimeoutErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.InvalidNodeTypeError, (jSError) => new InvalidNodeTypeErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.DataCloneError, (jSError) => new DataCloneErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.EncodingError, (jSError) => new EncodingErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NotReadableError, (jSError) => new NotReadableErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.UnknownError, (jSError) => new UnknownErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.ConstraintError, (jSError) => new ConstraintErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.DataError, (jSError) => new DataErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.TransactionInactiveError, (jSError) => new TransactionInactiveErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.ReadOnlyError, (jSError) => new ReadOnlyErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.VersionError, (jSError) => new VersionErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.OperationError, (jSError) => new OperationErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.NotAllowedError, (jSError) => new NotAllowedErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { DOMException.OptOutError, (jSError) => new OptOutErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { "EvalError", (jSError) => new EvalErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { "RangeError", (jSError) => new RangeErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { "ReferenceError", (jSError) => new ReferenceErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { "SyntaxError", (jSError) => new TypeErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { "TypeError", (jSError) => new TypeErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
        { "URIError", (jSError) => new URIErrorException(jSError.Message, jSError.Stack, jSError.InnerException) },
    });
}
