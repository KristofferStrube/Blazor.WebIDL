using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Objects implementing the <see cref="IReadWriteMapLike{TMap}"/> interface are maplike and have an ordered map of key–value pairs, initially empty, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
public interface IReadWriteMapLike<TMap> : IJSWrapper, IReadonlyMapLike<TMap> where TMap : IReadWriteMapLike<TMap> { }

/// <summary>
/// Objects implementing the <see cref="IReadWriteMapLike{TMap, TKey, TValue}"/> interface are maplike and have an ordered map of key–value pairs, initially empty, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
public interface IReadWriteMapLike<TMap, TKey, TValue> : IJSWrapper, IReadonlyMapLike<TMap, TKey, TValue> where TMap : IReadWriteMapLike<TMap, TKey, TValue> { }

/// <summary>
/// Extensions used to access members of objects that implement <see cref="IReadWriteMapLike{TMap, TKey, TValue}"/>.
/// </summary>
public static class IReadWriteMapLikeExtensions
{
    public static async Task<ulong> GetSizeAsync<TMap>(this IReadWriteMapLike<TMap> map) where TMap : IReadWriteMapLike<TMap>
    {
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", map.JSReference, "size");
    }

    public static async Task SetAsync<TMap, TKey, TValue>(this IReadWriteMapLike<TMap> map, TKey key, TValue value) where TMap : IReadWriteMapLike<TMap>
    {
        await map.JSReference.InvokeVoidAsync("set", key, value);
    }
}

/// <summary>
/// Extensions used to access members of objects that implement <see cref="IReadWriteMapLike{TMap, TKey, TValue}"/>.
/// </summary>
public static class IReadWriteTypedMapLikeExtensions
{
    public static async Task<ulong> GetSizeAsync<TMap, TKey, TValue>(this IReadWriteMapLike<TMap, TKey, TValue> map) where TMap : IReadWriteMapLike<TMap, TKey, TValue>
    {
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", map.JSReference, "size");
    }

    public static async Task SetAsync<TMap, TKey, TValue>(this IReadWriteMapLike<TMap, TKey, TValue> map, TKey key, TValue value) where TMap : IReadWriteMapLike<TMap, TKey, TValue>
    {
        await map.JSReference.InvokeVoidAsync("set", key, value);
    }
}
