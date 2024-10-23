using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Objects implementing the <see cref="IReadonlyMapLike{TMap}"/> interface are maplike and have an ordered map of key–value pairs, initially empty, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
public interface IReadonlyMapLike<TMap> : IJSWrapper where TMap : IReadonlyMapLike<TMap> { }

/// <summary>
/// Objects implementing the <see cref="IReadonlyMapLike{TMap, TKey, TValue}"/> interface are maplike and have an ordered map of key–value pairs, initially empty, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
public interface IReadonlyMapLike<TMap, TKey, TValue> : IJSWrapper where TMap : IReadonlyMapLike<TMap, TKey, TValue> { }


/// <summary>
/// Extensions used to access members of objects that implement <see cref="IReadonlyMapLike{TMap}"/>.
/// </summary>
public static class IReadonlyMapLikeExtensions
{
    public static async Task<ulong> GetSizeAsync<TMap>(this IReadonlyMapLike<TMap> map) where TMap : IReadonlyMapLike<TMap>
    {
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", map.JSReference, "size");
    }

    public static async Task<Iterator<Pair>> EntriesAsync<TMap>(this IReadonlyMapLike<TMap> map) where TMap : IReadonlyMapLike<TMap>
    {
        return await Iterator<Pair>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
    }
}


/// <summary>
/// Extensions used to access members of objects that implement <see cref="IReadonlyMapLike{TMap, TKey, TValue}"/>.
/// </summary>
public static class IReadonlyTypedMapLikeExtensions
{
    public static async Task<ulong> GetSizeAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", map.JSReference, "size");
    }

    public static async Task<Iterator<Pair<TKey, TValue>>> EntriesAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TKey : IJSCreatable<TKey> where TValue : IJSCreatable<TValue>
    {
        return await Iterator<Pair<TKey, TValue>>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
    }

    public static async Task<Iterator<StructKeyJSCreatableValuePair<TKey, TValue>>> EntriesStructKeyJSCreatableValueAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TKey : struct where TValue : IJSCreatable<TValue>
    {
        return await Iterator<StructKeyJSCreatableValuePair<TKey, TValue>>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
    }

    public static async Task<Iterator<StructKeyStructValuePair<TKey, TValue>>> EntriesStructKeyStructValueAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TKey : struct where TValue : struct
    {
        return await Iterator<StructKeyStructValuePair<TKey, TValue>>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("entries"), new CreationOptions() { DisposesJSReference = true });
    }

    public static async Task<StructIterator<TKey>> KeysAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TKey : struct
    {
        return await StructIterator<TKey>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("keys"), new() { DisposesJSReference = true });
    }

    public static async Task<Iterator<TValue>> ValuesAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : IJSCreatable<TValue>
    {
        return await Iterator<TValue>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
    }

    public static async Task<StructIterator<TValue>> ValuesAsStructsAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : struct
    {
        return await StructIterator<TValue>.CreateAsync(map.JSRuntime, await map.JSReference.InvokeAsync<IJSObjectReference>("values"), new() { DisposesJSReference = true });
    }

    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<Task> function) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        Callback callback = new(function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithNoArguments", map.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<TValue, Task> function) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : IJSCreatable<TValue>
    {
        Callback<TValue> callback = new(map.JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithOneArgument", map.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<TValue, TKey, Task> function) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : IJSCreatable<TValue> where TKey : struct
    {
        JSCreatableStructCallback<TValue, TKey> callback = new(map.JSRuntime, function);
        using var callbackObjRef = DotNetObjectReference.Create(callback);
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        await helper.InvokeVoidAsync("forEachWithTwoArguments", map.JSReference, callbackObjRef);
    }

    public static async Task ForEachAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, Func<TValue, TKey, IReadonlyMapLike<TMap, TKey, TValue>, Task> function) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : IJSCreatable<TValue> where TKey : struct
    {
        await map.ForEachAsync(async (value, key) => await function(value, key, map));
    }

    public static async Task<TValue> GetAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, TKey key) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : IJSCreatable<TValue>
    {
        IJSObjectReference jSInstance = await map.JSReference.InvokeAsync<IJSObjectReference>("get", key);
        return await TValue.CreateAsync(map.JSRuntime, jSInstance);
    }

    public static async Task<TValue> GetStructAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, TKey key) where TMap : IReadonlyMapLike<TMap, TKey, TValue> where TValue : struct
    {
        return await map.JSReference.InvokeAsync<TValue>("get", key);
    }

    public static async Task<bool> HasAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map, TKey key) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        return await map.JSReference.InvokeAsync<bool>("has", key);
    }
}
