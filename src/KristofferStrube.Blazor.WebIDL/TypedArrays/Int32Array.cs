using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of ints.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Int32Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Int32Array : TypedArray<int, Int32Array>, IJSCreatable<Int32Array>
{
    /// <inheritdoc/>
    public static Task<Int32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Int32Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Int32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Int32Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Int32Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
