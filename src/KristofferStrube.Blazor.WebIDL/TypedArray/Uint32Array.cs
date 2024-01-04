using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of uints.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint16Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint32Array : TypedArray<uint, Uint32Array>, IJSCreatable<Uint32Array>
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="Uint32Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Uint32Array"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Uint32Array"/>.</returns>
    public static Task<Uint32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint32Array(jSRuntime, jSReference));
    }
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="Uint32Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Uint32Array"/>.</param>
    protected Uint32Array(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }
}
