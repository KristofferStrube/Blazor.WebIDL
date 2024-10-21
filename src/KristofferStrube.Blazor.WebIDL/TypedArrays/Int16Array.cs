using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of shorts.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Int16Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Int16Array : TypedArray<short, Int16Array>, IJSCreatable<Int16Array>
{
    /// <inheritdoc/>
    public static Task<Int16Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Int16Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Int16Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Int16Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Int16Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }
}
