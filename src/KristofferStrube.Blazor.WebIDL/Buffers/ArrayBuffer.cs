using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An object that holds a pointer (which can be null) to a buffer of a fixed number of bytes.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/structured-data.html#sec-arraybuffer-objects">See the API definition here</see>.</remarks>
[IJSWrapperConverter]
public class ArrayBuffer : IArrayBuffer, IJSCreatable<ArrayBuffer>, ITransferable
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
    public static Task<ArrayBuffer> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new ArrayBuffer(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static Task<ArrayBuffer> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return Task.FromResult(new ArrayBuffer(jSRuntime, jSReference, options));
    }

    /// <summary>
    /// Creates an <see cref="ArrayBuffer"/> using the standard constructor.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="length">The initial length of the <see cref="ArrayBuffer"/></param>
    /// <returns></returns>
    public static async Task<ArrayBuffer> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructArrayBuffer", length);
        return new ArrayBuffer(jSRuntime, jSInstance, new() { DisposeOfJSReference = true });
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="ArrayBuffer"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="ArrayBuffer"/>.</param>
    /// <param name="options">The options for constructing this wrapper</param>
    protected ArrayBuffer(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
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
