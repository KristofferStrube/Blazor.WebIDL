using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A common interface for all classes that wrap a JS object which is accessible with in-process interop which also exposes a method to create an instance of it.
/// </summary>
/// <typeparam name="TInProcess"></typeparam>
/// <typeparam name="T"></typeparam>
public interface IJSInProcessCreatable<TInProcess, T> : IJSCreatable<T> where TInProcess : IJSInProcessCreatable<TInProcess, T> where T : IJSCreatable<T>
{
    /// <summary>
    /// An <see cref="IJSInProcessObjectReference"/> to the object that is being wrapped.
    /// </summary>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <summary>
    /// Constructs an in-process wrapper instance for an equivalent JS instance.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    public static abstract Task<TInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference);

    /// <summary>
    /// Constructs an in-process wrapper instance for an equivalent JS instance with the option for configuring how the wrapper is constructed.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    /// <param name="options">The options for constructing this wrapper</param>
    public static abstract Task<TInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options);
}
