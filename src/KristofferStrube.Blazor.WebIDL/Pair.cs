using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class Pair<TKey, TValue> : IJSWrapper<Pair<TKey, TValue>> where TKey : IJSWrapper<TKey> where TValue : IJSWrapper<TValue>
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

    public static async Task<Pair<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Pair<TKey, TValue>(jSRuntime, jSReference));
    }

    public Pair(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
    }

    public async Task<TKey> GetKeyAsync()
    {
        var helper = await helperTask.Value;
        return await TKey.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 0));
    }

    public async Task<TKey> GetValueAsync()
    {
        var helper = await helperTask.Value;
        return await TKey.CreateAsync(JSRuntime, await helper.InvokeAsync<IJSObjectReference>("getAttribute", JSReference, 1));
    }
}