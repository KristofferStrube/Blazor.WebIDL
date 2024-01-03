using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of IEEE 754 floating point numbers of 4 bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Float32Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Float32Array : TypedArray<float, Float32Array>, IJSCreatable<Float32Array>
{
    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Float32Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Float32Array"/>.</param>
    /// <returns>A wrapper instance for a <see cref="Float32Array"/>.</returns>
    public static Task<Float32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Float32Array(jSRuntime, jSReference));
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="Float32Array"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="Float32Array"/>.</param>
    protected Float32Array(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }
}
