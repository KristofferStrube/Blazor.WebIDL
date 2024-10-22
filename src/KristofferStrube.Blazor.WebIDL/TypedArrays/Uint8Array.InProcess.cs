using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A view on to a buffer type instance that exposes it as an array of bytes.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-Uint8Array">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class Uint8ArrayInProcess : Uint8Array, ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    public IJSInProcessObjectReference InProcessHelper { get; }

    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <inheritdoc/>
    public static async Task<Uint8ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<Uint8ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <inheritdoc/>
    public static new async Task<Uint8ArrayInProcess> CreateAsync(IJSRuntime jSRuntime)
    {
        return await ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.CreateInternalAsync(jSRuntime);
    }

    /// <inheritdoc/>
    public static new async Task<Uint8ArrayInProcess> CreateAsync<TFromElement, TFromTypedArray>(IJSRuntime jSRuntime, TypedArray<TFromElement, TFromTypedArray> typedArray) where TFromTypedArray : IJSCreatable<TFromTypedArray>
    {
        return await ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.CreateInternalAsync(jSRuntime, typedArray);
    }

    /// <inheritdoc/>
    public static new async Task<Uint8ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, IArrayBuffer buffer, long? byteOffset, long? length)
    {
        return await ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.CreateInternalAsync(jSRuntime, buffer, byteOffset, length);
    }

    /// <inheritdoc/>
    public static new async Task<Uint8ArrayInProcess> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        return await ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.CreateInternalAsync(jSRuntime, length);
    }

    /// <inheritdoc cref="IJSInProcessCreatable{TInProcess, T}.CreateAsync(IJSRuntime, IJSInProcessObjectReference, CreationOptions)"/>
    protected Uint8ArrayInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        this.InProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }

    /// <inheritdoc/>
    public new async Task<IArrayBufferInProcess> GetBufferAsync()
    {
        return await ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.GetBufferAsync(this);
    }

    /// <inheritdoc/>
    public byte At(long index)
    {
        return ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.At(this, index);
    }

    /// <inheritdoc/>
    public long Length => ITypedArrayInProcess<byte, Uint8ArrayInProcess, Uint8Array>.GetLength(this);

    /// <summary>
    /// Gets the array as a .NET byte array.
    /// </summary>
    public byte[] AsArray => JSReference.Invoke<byte[]>("valueOf");

    /// <inheritdoc/>
    public new async ValueTask DisposeAsync()
    {
        await InProcessHelper.DisposeAsync();
        await IJSWrapper.DisposeJSReference(this);
        await base.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}
