using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An object that holds a pointer (which cannot be null) to a shared buffer of a fixed number of bytes
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/structured-data.html#sec-sharedarraybuffer-objects">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class SharedArrayBufferInProcess : SharedArrayBuffer, IArrayBufferInProcess, IJSInProcessCreatable<SharedArrayBufferInProcess, SharedArrayBuffer>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected IJSInProcessObjectReference inProcessHelper;

    /// <inheritdoc/>
    public new IJSInProcessObjectReference JSReference { get; }

    /// <inheritdoc/>
    public static async Task<SharedArrayBufferInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference)
    {
        return await CreateAsync(jSRuntime, jSReference, new());
    }

    /// <inheritdoc/>
    public static async Task<SharedArrayBufferInProcess> CreateAsync(IJSRuntime jSRuntime, IJSInProcessObjectReference jSReference, CreationOptions options)
    {
        IJSInProcessObjectReference inProcessHelper = await jSRuntime.GetInProcessHelperAsync();
        return new SharedArrayBufferInProcess(jSRuntime, inProcessHelper, jSReference, options);
    }

    /// <inheritdoc/>
    protected SharedArrayBufferInProcess(IJSRuntime jSRuntime, IJSInProcessObjectReference inProcessHelper, IJSInProcessObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
        this.inProcessHelper = inProcessHelper;
        JSReference = jSReference;
    }
}
