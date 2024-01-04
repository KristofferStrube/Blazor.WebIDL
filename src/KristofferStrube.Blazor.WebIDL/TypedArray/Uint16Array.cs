using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of ushorts.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint16Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint16Array : TypedArray<ushort, Uint16Array>, IJSCreatable<Uint16Array>
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="Uint16Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Uint16Array"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Uint16Array"/>.</returns>
    public static Task<Uint16Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint16Array(jSRuntime, jSReference));
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="Uint16Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Uint16Array"/>.</param>
    protected Uint16Array(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }
}
