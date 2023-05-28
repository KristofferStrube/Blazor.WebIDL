using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A common interface for all classes that wrap some JS object.
/// </summary>
public interface IJSWrapper
{
    /// <summary>
    /// A <see cref="IJSObjectReference"/> to the object that is being wrapped.
    /// </summary>
    public IJSObjectReference JSReference { get; }

    /// <summary>
    /// The JSRuntime that is used to invoke indirect functions on the <see cref="JSReference"/>.
    /// </summary>
    public IJSRuntime JSRuntime { get; }
}