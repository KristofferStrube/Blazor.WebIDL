using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A common interface for all classes that wrap some JS object.
/// </summary>
public interface IJSWrapper : IAsyncDisposable
{
    /// <summary>
    /// An <see cref="IJSObjectReference"/> to the object that is being wrapped.
    /// </summary>
    public IJSObjectReference JSReference { get; }

    /// <summary>
    /// The JSRuntime that is used to invoke indirect functions on the <see cref="JSReference"/>.
    /// </summary>
    public IJSRuntime JSRuntime { get; }

    /// <summary>
    /// Whether the wrapper disposes of the <see cref="JSReference"/> when the wrapper is disposed of.
    /// </summary>
    public bool DisposesJSReference { get; }

    /// <summary>
    /// Disposes the wrapper.
    /// </summary>
    public abstract new ValueTask DisposeAsync();

    /// <summary>
    /// Disposes the underlying JSReference if <see cref="DisposesJSReference"/> is set to <see langword="true"/>.
    /// </summary>
    /// <returns></returns>
    public static async ValueTask DisposeJSReference(IJSWrapper wrapper)
    {
        if (wrapper.DisposesJSReference)
        {
            await wrapper.JSReference.DisposeAsync();
        }
    }
}