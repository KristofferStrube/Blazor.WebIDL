using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of ushorts.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint16Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint16Array : TypedArray<ushort, Uint16Array>, IJSCreatable<Uint16Array>
{
    /// <inheritdoc/>
    public static Task<Uint16Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint16Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Uint16Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Uint16Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Uint16Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
