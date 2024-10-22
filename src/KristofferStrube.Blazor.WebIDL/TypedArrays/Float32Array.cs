using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of IEEE 754 floating point numbers of 4 bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Float32Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Float32Array : TypedArray<float, Float32Array>, IJSCreatable<Float32Array>
{
    /// <inheritdoc/>
    public static Task<Float32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Float32Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Float32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Float32Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Float32Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
