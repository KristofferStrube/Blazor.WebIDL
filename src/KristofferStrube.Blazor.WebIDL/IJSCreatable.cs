using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A common interface for all classes that wrap a JS object which also exposes a method to create an instance of it.
/// </summary>
/// <typeparam name="T">The type of wrapper that the wrapper can create.</typeparam>
public interface IJSCreatable<T> : IJSWrapper where T : IJSCreatable<T>
{
    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <typeparamref name="T"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    public static abstract Task<T> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference);

    /// <summary>
    /// Constructs a wrapper instance for an equivalent JS instance of a <typeparamref name="T"/> with the option for configuring how the wrapper is constructed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">The options for constructing this wrapper</param>
    public static abstract Task<T> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options);
}