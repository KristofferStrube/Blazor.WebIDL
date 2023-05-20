using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Represents a reference to a JavaScript object. This instance differs from <see cref="IJSObjectReference"/> by raising typed errors with more information if the invocation fails.
/// </summary>
public interface IErrorHandlingJSObjectReference : IJSObjectReference
{
    /// <summary>
    /// The inner <see cref="IJSObjectReference"/> that invocations are called on.
    /// </summary>
    IJSObjectReference JSReference { get; }

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
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
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous invocation operation.</returns>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// <para>
    /// <see cref="JSRuntime"/> will apply timeouts to this operation based on the value configured in <see cref="JSRuntime.DefaultAsyncTimeout"/>. To dispatch a call with a different, or no timeout,
    /// consider using <see cref="InvokeAsync{TValue}(string, CancellationToken, object[])" />.
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>someScope.someFunction</c> on the target instance.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    new ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function asynchronously.
    /// </summary>
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>someScope.someFunction</c> on the target instance.</param>
    /// <param name="cancellationToken">
    /// A cancellation token to signal the cancellation of the operation. Specifying this parameter will override any default cancellations such as due to timeouts
    /// (<see cref="JSRuntime.DefaultAsyncTimeout"/>) from being applied.
    /// </param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    new ValueTask<TValue> InvokeAsync<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, CancellationToken cancellationToken, object?[]? args);

    /// <summary>
    /// Disposes of the reference.
    /// </summary>
    new ValueTask DisposeAsync();
}