using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of IEEE 754 floating point numbers of 8 bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Float64Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Float64Array : TypedArray<double, Float64Array>, IJSCreatable<Float64Array>
{
    /// <inheritdoc/>
    public static Task<Float64Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Float64Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Float64Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Float64Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Float64Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
