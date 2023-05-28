using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A common interface for all classes that wrap a JS object which also exposes a method to create an instance of it.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IJSCreatable<T> : IJSWrapper where T : IJSCreatable<T>
{
    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    public static abstract Task<T> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference);
}