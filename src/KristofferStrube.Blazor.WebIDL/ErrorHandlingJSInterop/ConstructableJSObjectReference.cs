using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A <see cref="JSObjectReference"/> that has a constructor that is accessible from Blazor.WebIDL.
/// </summary>
internal class ConstructableJSObjectReference : JSObjectReference
{
    /// <inheritdoc/>
    internal ConstructableJSObjectReference(JSRuntime jsRuntime, long id) : base(jsRuntime, id) { }
}
