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

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of an <see cref="SharedArrayBuffer"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="SharedArrayBuffer"/>.</param>
    /// <returns>A wrapper instance for a <see cref="SharedArrayBuffer"/>.</returns>
    public static Task<SharedArrayBuffer> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new SharedArrayBuffer(jSRuntime, jSReference));
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
        return new SharedArrayBuffer(jSRuntime, jSInstance);
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="SharedArrayBuffer"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="SharedArrayBuffer"/>.</param>
    protected SharedArrayBuffer(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
    }
}
