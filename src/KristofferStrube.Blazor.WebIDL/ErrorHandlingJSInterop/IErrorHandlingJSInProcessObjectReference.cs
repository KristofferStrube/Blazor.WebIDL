using KristofferStrube.Blazor.WebIDL.Exceptions;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Represents a reference to a JavaScript object whose functions can be invoked synchronously. This instance differs from <see cref="IJSInProcessObjectReference"/> by raising typed errors with more information if the invocation fails.
/// </summary>
public interface IErrorHandlingJSInProcessObjectReference : IErrorHandlingJSObjectReference, IJSInProcessObjectReference
{
    /// <summary>
    /// Invokes the specified JavaScript function synchronously.
    /// </summary>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>someScope.someFunction</c> on the target instance.</param>
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
    /// <typeparam name="TValue">The JSON-serializable return type.</typeparam>
    /// <param name="identifier">An identifier for the function to invoke. For example, the value <c>"someScope.someFunction"</c> will invoke the function <c>someScope.someFunction</c> on the target instance.</param>
    /// <param name="args">JSON-serializable arguments.</param>
    /// <exception cref="DOMException" />
    /// <exception cref="EvalErrorException" />
    /// <exception cref="RangeErrorException" />
    /// <exception cref="ReferenceErrorException" />
    /// <exception cref="TypeErrorException" />
    /// <exception cref="URIErrorException" />
    /// <exception cref="JSException" />
    /// <returns>An instance of <typeparamref name="TValue"/> obtained by JSON-deserializing the return value.</returns>
    new TValue Invoke<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicProperties)] TValue>(string identifier, params object?[]? args);
}
