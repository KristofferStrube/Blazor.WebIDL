using Microsoft.JSInterop;
using System.Xml.Linq;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint8Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint8Array : TypedArray<byte, Uint8Array>, IJSCreatable<Uint8Array>
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="Uint8Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Uint8Array"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Uint8Array"/>.</returns>
    public new static Task<Uint8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint8Array(jSRuntime, jSReference));
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="Uint8Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Uint8Array"/>.</param>
    protected Uint8Array(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }

    /// <summary>
    /// Gets the array as an .NET byte array.
    /// </summary>
    public async Task<byte[]> GetAsArrayAsync()
    {
        return await JSReference.InvokeAsync<byte[]>("valueOf");
    }
}
