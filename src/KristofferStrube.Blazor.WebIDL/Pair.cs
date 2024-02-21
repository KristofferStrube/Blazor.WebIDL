using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

[IJSWrapperConverter]
public class Pair : IJSCreatable<Pair>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }
    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <inheritdoc/>
    public static async Task<Pair> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Pair(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<Pair> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new Pair(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)" />
    protected Pair(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposeOfJSReference;
    }

    public async Task<TKey> GetKeyAsync<TKey>() where TKey : IJSCreatable<TKey>
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposeOfJSReference = true
        };

        return await TKey.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 0), options);
    }

    public async Task<TValue> GetValueAsync<TValue>() where TValue : IJSCreatable<TValue>
    {
        IJSObjectReference helper = await helperTask.Value;
        CreationOptions options = new()
        {
            DisposeOfJSReference = true
        };

        return await TValue.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 1), options);
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