using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of uints.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint16Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint32Array : TypedArray<uint, Uint32Array>, IJSCreatable<Uint32Array>
{
    /// <inheritdoc/>
    public static Task<Uint32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint32Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Uint32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Uint32Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Uint32Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
