using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Represents an instance of a JavaScript runtime to which calls may be dispatched. This instance differs from <see cref="IJSRuntime"/> by raising typed errors with more information if the invocation fails.
/// </summary>
public class ErrorHandlingJSRuntime : IAsyncDisposable, IErrorHandlingJSRuntime
{
    private readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <summary>
    /// Creates a new instance of the JavaScript runtime wrapper. Will often be implicitly called when registered as a service.
    /// </summary>
    /// <param name="jSRuntime"></param>
    public ErrorHandlingJSRuntime(IJSRuntime jSRuntime)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
    }

    /// <inheritdoc />
    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        await InvokeVoidAsync(identifier, CancellationToken.None, args);
    }

    /// <inheritdoc />
    public async ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        await InvokeAsync<object>(identifier, cancellationToken, args);
    }

    /// <inheritdoc />
    public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args)
    {
        return await InvokeAsync<TValue>(identifier, CancellationToken.None, args);
    }

    /// <inheritdoc />
    public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        var helper = await helperTask.Value;
        try
        {
            return await helper.InvokeAsync<TValue>("callAsyncGlobalMethod", cancellationToken, identifier, args);
        }
        catch (JSException exception)
        {
            if (UnpackMessageOfExeption(exception) is not (string name, string message))
            {
                throw;
            }
            throw MapToWebIDLException(name, message, exception);
        }
    }

    /// <inheritdoc />
    public async ValueTask InvokeVoidOnInstanceAsync(IJSObjectReference jSInstance, string identifier, params object?[]? args)
    {
        await InvokeVoidOnInstanceAsync(jSInstance, identifier, CancellationToken.None, args);
    }

    /// <inheritdoc />
    public async ValueTask InvokeVoidOnInstanceAsync(IJSObjectReference jSInstance, string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        await InvokeOnInstanceAsync<object>(jSInstance, identifier, cancellationToken, args);
    }

    /// <inheritdoc />
    public async ValueTask<TValue> InvokeOnInstanceAsync<TValue>(IJSObjectReference jSInstance, string identifier, params object?[]? args)
    {
        return await InvokeOnInstanceAsync<TValue>(jSInstance, identifier, CancellationToken.None, args);
    }

    /// <inheritdoc />
    public async ValueTask<TValue> InvokeOnInstanceAsync<TValue>(IJSObjectReference jSInstance, string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        var helper = await helperTask.Value;
        try
        {
            return await helper.InvokeAsync<TValue>("callAsyncInstanceMethod", cancellationToken, jSInstance, identifier, args);
        }
        catch (JSException exception)
        {
            if (UnpackMessageOfExeption(exception) is not (string name, string message))
            {
                throw;
            }
            throw MapToWebIDLException(name, message, exception);
        }
    }

    private static (string name, string message)? UnpackMessageOfExeption(JSException exception)
    {
        var packedException = exception.Message;
        if (packedException.EndsWith("undefined"))
        {
            packedException = packedException[..^9];
        }
        if (packedException.Trim().Split(":") is not [string name, .. string[] { Length: >= 1 } rest])
        {
            return null;
        }
        var message = string.Join(":", rest);
        return (name, message);
    }

    private static WebIDLException MapToWebIDLException(string name, string message, JSException exception) => name switch
    {
        DOMException.HierarchyRequestError => new HierarchyRequestErrorException(message, exception),
        DOMException.WrongDocumentError => new WrongDocumentErrorException(message, exception),
        DOMException.InvalidCharacterError => new InvalidCharacterErrorException(message, exception),
        DOMException.NoModificationAllowedError => new NoModificationAllowedErrorException(message, exception),
        DOMException.NotFoundError => new NotFoundErrorException(message, exception),
        DOMException.NotSupportedError => new NotSupportedErrorException(message, exception),
        DOMException.InUseAttributeError => new InUseAttributeErrorException(message, exception),
        DOMException.InvalidStateError => new InvalidStateErrorException(message, exception),
        DOMException.SyntaxError => new SyntaxErrorException(message, exception),
        DOMException.InvalidModificationError => new InvalidModificationErrorException(message, exception),
        DOMException.NamespaceError => new NamespaceErrorException(message, exception),
        DOMException.SecurityError => new SecurityErrorException(message, exception),
        DOMException.NetworkError => new NetworkErrorException(message, exception),
        DOMException.AbortError => new AbortErrorException(message, exception),
        DOMException.QuotaExceededError => new QuotaExceededErrorException(message, exception),
        DOMException.TimeoutError => new TimeoutErrorException(message, exception),
        DOMException.InvalidNodeTypeError => new InvalidNodeTypeErrorException(message, exception),
        DOMException.DataCloneError => new DataCloneErrorException(message, exception),
        DOMException.EncodingError => new EncodingErrorException(message, exception),
        DOMException.NotReadableError => new NotReadableErrorException(message, exception),
        DOMException.UnknownError => new UnknownErrorException(message, exception),
        DOMException.ConstraintError => new ConstraintErrorException(message, exception),
        DOMException.DataError => new DataErrorException(message, exception),
        DOMException.TransactionInactiveError => new TransactionInactiveErrorException(message, exception),
        DOMException.ReadOnlyError => new ReadOnlyErrorException(message, exception),
        DOMException.VersionError => new VersionErrorException(message, exception),
        DOMException.OperationError => new OperationErrorException(message, exception),
        DOMException.NotAllowedError => new NotAllowedErrorException(message, exception),
        DOMException.OptOutError => new OptOutErrorException(message, exception),
        "EvalError" => new EvalErrorException(message, exception),
        "RangeError" => new RangeErrorException(message, exception),
        "ReferenceError" => new ReferenceErrorException(message, exception),
        "TypeError" => new TypeErrorException(message, exception),
        "URIError" => new URIErrorException(message, exception),
        _ => new WebIDLException($"{name}: \"{message}\"", exception),
    };

    /// <summary>
    /// Disposes of the service.
    /// </summary>
    /// <returns></returns>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}
