using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IReadonlySetlike<T> : IJSWrapper, IAsyncEnumerable<T> where T : IJSWrapper<T>
{
    public async Task<Iterator<Pair<T, T>>> EntriesAsync()
    {
        return await Iterator<Pair<T, T>>.CreateAsync(JSRuntime, await JSReference.InvokeAsync<IJSObjectReference>("entries"));
    }

    public async Task ForEachAsync(Func<Task> function)
    {
        var callback = new Callback(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        var helper = await JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithNoArguments", JSReference, callbackObjRef);
    }

    public async Task ForEachAsync(Func<T, Task> function)
    {
        var callback = new Callback<T>(JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        var helper = await JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithOneArgument", JSReference, callbackObjRef);
    }

    public async Task ForEachAsync(Func<T, T, Task> function)
    {
        var callback = new Callback<T, T>(JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        var helper = await JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithTwoArguments", JSReference, callbackObjRef);
    }

    public async Task ForEachAsync(Func<T, T, IReadonlySetlike<T>, Task> function)
    {
        var callback = new Callback<T, T>(JSRuntime, (value, key) => function(value, key, this));
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        var helper = await JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithTwoArguments", JSReference, callbackObjRef);
    }

    public async Task<bool> HasAsync(T element)
    {
        return await JSReference.InvokeAsync<bool>("has", element.JSReference);
    }

    public async Task<Iterator<T>> ValuesAsync()
    {
        return await Iterator<T>.CreateAsync(JSRuntime, await JSReference.InvokeAsync<IJSObjectReference>("values"));
    }

    public async Task<Iterator<T>> KeysAsync() => await ValuesAsync();

    public async Task<IAsyncEnumerator<T>> IteratorAsync()
    {
        return await Task.FromResult<IAsyncEnumerator<T>>(default!);
    }

    public async Task<ulong> GetSizeAsync()
    {
        var helper = await JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", JSReference, "size");
    }
}
