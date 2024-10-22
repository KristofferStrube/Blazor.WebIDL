using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of uints.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint32Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint32ArrayInProcess : Uint32Array, ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    public IJSInProcessObjectReference InProcessHelper { get; }

    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <inheritdoc/>
    public static async Task<Uint32ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<Uint32ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <inheritdoc/>
    public static new async Task<Uint32ArrayInProcess> CreateAsync(IJSRuntime jSRuntime)
    {
        return await ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.CreateInternalAsync(jSRuntime);
    }

    /// <inheritdoc/>
    public static new async Task<Uint32ArrayInProcess> CreateAsync<TFromElement, TFromTypedArray>(IJSRuntime jSRuntime, TypedArray<TFromElement, TFromTypedArray> typedArray) where TFromTypedArray : IJSCreatable<TFromTypedArray>
    {
        return await ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.CreateInternalAsync(jSRuntime, typedArray);
    }

    /// <inheritdoc/>
    public static new async Task<Uint32ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, IArrayBuffer buffer, long? byteOffset, long? length)
    {
        return await ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.CreateInternalAsync(jSRuntime, buffer, byteOffset, length);
    }

    /// <inheritdoc/>
    public static new async Task<Uint32ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        return await ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.CreateInternalAsync(jSRuntime, length);
    }

    /// <inheritdoc cref="IJSInProcessCreatable{TInProcess, T}.CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected Uint32ArrayInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        this.InProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <inheritdoc/>
    public new async Task<IArrayBufferInProcess> GetBufferAsync()
    {
        return await ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.GetBufferAsync(this);
    }

    /// <inheritdoc/>
    public uint At(long index)
    {
        return ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.At(this, index);
    }

    /// <inheritdoc/>
    public long Length => ITypedArrayInProcess<uint, Uint32ArrayInProcess, Uint32Array>.GetLength(this);
}
