using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

[IJSWrapperConverter]
public class Pair : IJSCreatable<Pair>, IAsyncDisposable
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

    public static async Task<Pair> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Pair(jSRuntime, jSReference));
    }

    public Pair(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
    }

    public async Task<TKey> GetKeyAsync<TKey>() where TKey : IJSCreatable<TKey>
    {
        IJSObjectReference helper = await helperTask.Value;
        return await TKey.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 0));
    }

    public async Task<TValue> GetValueAsync<TValue>() where TValue : IJSCreatable<TValue>
    {
        IJSObjectReference helper = await helperTask.Value;
        return await TValue.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 1));
    }

    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}