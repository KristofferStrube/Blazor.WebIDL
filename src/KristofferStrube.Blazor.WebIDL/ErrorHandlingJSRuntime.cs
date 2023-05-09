using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class ErrorHandlingJSRuntime : IAsyncDisposable, IJSRuntime
{
    private readonly Lazy<Task<IJSObjectReference>> helperTask;

    public ErrorHandlingJSRuntime(IJSRuntime jSRuntime)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    public async ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
    {
        await InvokeVoidAsync(identifier, CancellationToken.None, args);
    }
    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    public async ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        await InvokeAsync<object>(identifier, cancellationToken, args);
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// <para>
    /// <see cref="JSRuntime"/> will apply timeouts to this operation based on the value configured in <see cref="JSRuntime.DefaultAsyncTimeout"/>. To dispatch a call with a different timeout, or no timeout,
    /// consider using <see cref="InvokeAsync{TValue}(string, CancellationToken, object[])" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args)
    {
        return await InvokeAsync<TValue>(identifier, CancellationToken.None, args);
    }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    public async ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args)
    {
        var helper = await helperTask.Value;
        try
        {
            return await helper.InvokeAsync<TValue>("callGlobalAsyncMethod", cancellationToken, identifier, args);
        }
        catch (JSException exception)
        {
            var packedException = exception.Message;
            if (packedException.EndsWith("undefined"))
            {
                packedException = packedException[..^9];
            }
            if (packedException.Trim().Split(":") is not [string name, .. string[] { Length: >= 1 } rest])
            {
                throw;
            }
            var message = string.Join(":", rest);
            throw name switch
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
        }
    }

    private record Error(string Type, string Name, string Message);

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
