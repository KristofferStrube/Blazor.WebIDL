using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint8Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint8Array : TypedArray<byte, Uint8Array>, IJSCreatable<Uint8Array>
{
    /// <inheritdoc/>
    public static Task<Uint8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint8Array(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<Uint8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new Uint8Array(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Uint8Array(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

    /// <summary>
    /// Gets the array as a .NET byte array.
    /// </summary>
    public async Task<byte[]> GetAsArrayAsync()
    {
        return await JSReference.InvokeAsync<byte[]>("valueOf");
    }
}
