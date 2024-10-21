using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of signed bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Int8Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Int8Array : TypedArray<sbyte, Int8Array>, IJSCreatable<Int8Array>
{
    /// <inheritdoc/>
    public static Task<Int8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Int8Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Int8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Int8Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Int8Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
