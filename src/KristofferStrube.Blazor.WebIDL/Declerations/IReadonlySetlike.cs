using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IReadonlySetlike<TSet> : IJSWrapper where TSet : IReadonlySetlike<TSet> { }

public interface IReadonlySetlike<TSet, TElement> : IJSWrapper where TSet : IReadonlySetlike<TSet, TElement> { }

public static class IReadonlySetlikeExtensions
{
    public static async Task<Iterator<Pair>> EntriesAsync<TSet>(this TSet set) where TSet : IReadonlySetlike<TSet>
    {
        return await Iterator<Pair>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
    }

    public static async Task ForEachAsync<TSet>(this TSet set, Func<Task> function) where TSet : IReadonlySetlike<TSet>
    {
        Callback callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithNoArguments", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, T>(this TSet set, Func<T, Task> function) where TSet : IReadonlySetlike<TSet> where T : IJSCreatable<T>
    {
        Callback<T> callback = new(set.JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithOneArgument", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, T>(this TSet set, Func<T, T, Task> function) where TSet : IReadonlySetlike<TSet> where T : IJSCreatable<T>
    {
        Callback<T, T> callback = new(set.JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithTwoArguments", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, T>(this TSet set, Func<T, T, TSet, Task> function) where TSet : IReadonlySetlike<TSet> where T : IJSCreatable<T>
    {
        await set.ForEachAsync<TSet, T>(async (value, key) => await function(value, key, set));
    }

    public static async Task<bool> HasAsync<TSet, T>(this TSet set, T element) where TSet : IReadonlySetlike<TSet> where T : IJSWrapper
    {
        return await set.JSReference.InvokeAsync<bool>("has", element);
    }

    public static async Task<Iterator<T>> ValuesAsync<TSet, T>(this TSet set) where TSet : IReadonlySetlike<TSet> where T : IJSCreatable<T>
    {
        return await Iterator<T>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
    }

    public static async Task<Iterator<T>> KeysAsync<TSet, T>(this TSet set) where TSet : IReadonlySetlike<TSet> where T : IJSCreatable<T>
    {
        return await set.ValuesAsync<TSet, T>();
    }

    public static async Task<ulong> GetSizeAsync<TSet>(this TSet set) where TSet : IReadonlySetlike<TSet>
    {
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", set.JSReference, "size");
    }
}

public static class IReadonlyTypedSetlikeExtensions
{
    public static async Task<Iterator<Pair<TElement, TElement>>> EntriesAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSCreatable<TElement>
    {
        return await Iterator<Pair<TElement, TElement>>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
    }

    public static async Task ForEachAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, Func<Task> function) where TSet : IReadonlySetlike<TSet, TElement>
    {
        Callback callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithNoArguments", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, Func<TElement, Task> function) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSCreatable<TElement>
    {
        Callback<TElement> callback = new(set.JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithOneArgument", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, Func<TElement, TElement, Task> function) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSCreatable<TElement>
    {
        Callback<TElement, TElement> callback = new(set.JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithTwoArguments", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, Func<TElement, TElement, IReadonlySetlike<TSet, TElement>, Task> function) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSCreatable<TElement>
    {
        await set.ForEachAsync(async (value, key) => await function(value, key, set));
    }

    public static async Task<bool> HasAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set, TElement element) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSWrapper
    {
        return await set.JSReference.InvokeAsync<bool>("has", element);
    }

    public static async Task<Iterator<TElement>> ValuesAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSCreatable<TElement>
    {
        return await Iterator<TElement>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
    }

    public static async Task<Iterator<TElement>> KeysAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set) where TSet : IReadonlySetlike<TSet, TElement> where TElement : IJSCreatable<TElement>
    {
        return await Iterator<TElement>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("keys"), new() { DisposesJSReference = true });
    }

    public static async Task<ulong> GetSizeAsync<TSet, TElement>(this IReadonlySetlike<TSet, TElement> set) where TSet : IReadonlySetlike<TSet, TElement>
    {
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", set.JSReference, "size");
    }
}

public static class IReadonlySetlikeStructExtensions
{
    public static async Task ForEachAsync<TSet, T>(this TSet set, Func<T, Task> function) where TSet : IReadonlySetlike<TSet> where T : struct
    {
        StructCallback<T> callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithOneStructArgument", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, T>(this TSet set, Func<T, T, Task> function) where TSet : IReadonlySetlike<TSet> where T : struct
    {
        StructCallback<T, T> callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await set.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithTwoStructArguments", set.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TSet, T>(this TSet set, Func<T, T, TSet, Task> function) where TSet : IReadonlySetlike<TSet> where T : struct
    {
        await set.ForEachAsync<TSet, T>(async (value, key) => await function(value, key, set));
    }

    public static async Task<bool> HasAsync<TSet, T>(this TSet set, T element) where TSet : IReadonlySetlike<TSet> where T : struct
    {
        return await set.JSReference.InvokeAsync<bool>("has", element);
    }

    public static async Task<StructIterator<T>> ValuesAsync<TSet, T>(this TSet set) where TSet : IReadonlySetlike<TSet> where T : struct
    {
        return await StructIterator<T>.CreateAsync(set.JSRuntime, await set.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
    }

    public static async Task<StructIterator<T>> KeysAsync<TSet, T>(this TSet set) where TSet : IReadonlySetlike<TSet> where T : struct
    {
        return await ValuesAsync<TSet, T>(set);
    }
}