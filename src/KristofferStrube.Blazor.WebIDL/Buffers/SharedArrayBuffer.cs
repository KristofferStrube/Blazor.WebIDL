using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An object that holds a pointer (which cannot be null) to a shared buffer of a fixed number of bytes
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/structured-data.html#sec-sharedarraybuffer-objects">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class SharedArrayBuffer : IArrayBuffer, IJSCreatable<SharedArrayBuffer>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc/>
    public static Task<SharedArrayBuffer> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new SharedArrayBuffer(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<SharedArrayBuffer> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new SharedArrayBuffer(jSRuntime, jSReference, options));
    }

    /// <summary>
    /// Creates an <see cref="SharedArrayBuffer"/> using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="length">The initial length of the <see cref="SharedArrayBuffer"/></param>
    /// <returns></returns>
    public static async Task<SharedArrayBuffer> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructSharedArrayBuffer", length);
        return new SharedArrayBuffer(jSRuntime, jSInstance, new() { DisposeOfJSReference = true });
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected SharedArrayBuffer(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposeOfJSReference;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}
