using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Represents an instance of a JavaScript runtime to which calls may be dispatched. This instance differs from <see cref="IJSRuntime"/> by raising typed errors with more information if the invocation fails.
/// </summary>
public interface IErrorHandlingJSInProcessRuntime : IErrorHandlingJSRuntime, IJSInProcessRuntime
{
    /// <summary>
    /// Invokes the specified JavaScript function synchronously.
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
    void InvokeVoid(string identifier, params object?[]? args);

    /// <summary>
    /// Invokes the specified JavaScript function synchronously.
    /// </summary>
    /// <typeparam name="TResult">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>window.someScope.someFunction</c>.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>An instance of <typeparamref name="TResult"/> obtained by JSON-deserializing the return value.</returns>
    new TResult Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TResult>(string identifier, params object?[]? args);
}