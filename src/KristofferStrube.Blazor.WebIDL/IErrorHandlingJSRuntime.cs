using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Represents an instance of a JavaScript runtime to which calls may be dispatched. This instance differs from <see cref="IJSRuntime"/> by raising typed errors with more information if the invocation fails.
/// </summary>
public interface IErrorHandlingJSRuntime : IJSRuntime
{
    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    ValueTask InvokeVoidAsync(string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[]? args);

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
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    new ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args);

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
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    new ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously on the given instance.
    /// </summary>
    /// <param name="jSInstance">The JS instance that the method will be called on.</param>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>jSInstance.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    ValueTask InvokeVoidOnInstanceAsync(IJSObjectReference jSInstance, string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously on the given instance.
    /// </summary>
    /// <param name="jSInstance">The JS instance that the method will be called on.</param>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>jSInstance.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    ValueTask InvokeVoidOnInstanceAsync(IJSObjectReference jSInstance, string identifier, CancellationToken cancellationToken, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously on the given instance.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="jSInstance">The JS instance that the method will be called on.</param>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>jSInstance.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    ValueTask<TValue> InvokeOnInstanceAsync<TValue>(IJSObjectReference jSInstance, string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously on the given instance.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="jSInstance">The JS instance that the method will be called on.</param>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>jSInstance.someScope.someFunction</c>.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    ValueTask<TValue> InvokeOnInstanceAsync<TValue>(IJSObjectReference jSInstance, string identifier, CancellationToken cancellationToken, params object?[]? args);
}