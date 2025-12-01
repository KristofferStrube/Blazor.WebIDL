using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Objects implementing the <see cref="IReadWriteMapLike{TMap, TKey, TValue}"/> interface are maplike and have an ordered map of key–value pairs, initially empty, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
public interface IReadWriteMapLike<TMap, TKey, TValue> : IReadonlyMapLike<TMap, TKey, TValue> where TMap : IReadWriteMapLike<TMap, TKey, TValue> { }

/// <summary>
/// Extensions used to access members of objects that implement <see cref="IReadWriteMapLike{TMap, TKey, TValue}"/>.
/// </summary>
public static class IReadWriteMapLikeExtensions
{
    /// <summary>
    /// Sets the <paramref name="value"/> of a specific <paramref name="key"/> in the <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map</typeparam>
    /// <param name="map">The map to update.</param>
    /// <param name="key">The key of the entry that is updated or inserted.</param>
    /// <param name="value">The new value.</param>
    /// <returns></returns>
    public static async Task SetAsync<TMap, TKey, TValue>(this IReadWriteMapLike<TMap, TKey, TValue> map, TKey key, TValue value) where TMap : IReadWriteMapLike<TMap, TKey, TValue>
    {
        await map.JSReference.InvokeVoidAsync("set", key, value);
    }

    /// <summary>
    /// Clears the <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map</typeparam>
    /// <param name="map">The map that should be cleared.</param>
    /// <returns></returns>
    public static async Task ClearAsync<TMap, TKey, TValue>(this IReadWriteMapLike<TMap, TKey, TValue> map) where TMap : IReadWriteMapLike<TMap, TKey, TValue>
    {
        await map.JSReference.InvokeVoidAsync("clear");
    }

    /// <summary>
    /// Deletes the entry specified by a specific <paramref name="key"/> in the <paramref name="map"/>.
    /// </summary>
    /// <typeparam name="TMap">The type of the map.</typeparam>
    /// <typeparam name="TKey">The type of the keys of the map.</typeparam>
    /// <typeparam name="TValue">The type of the values in the map</typeparam>
    /// <param name="map">The map that should be updated.</param>
    /// <param name="key">The key for the entry that should be removed.</param>
    /// <returns><see langword="true"/> if it was in the map; else <see langword="false"/> if it was not in the map.</returns>
    public static async Task<bool> DeleteAsync<TMap, TKey, TValue>(this IReadWriteMapLike<TMap, TKey, TValue> map, TKey key) where TMap : IReadWriteMapLike<TMap, TKey, TValue>
    {
        return await map.JSReference.InvokeAsync<bool>("delete", key);
    }
}
