using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Objects implementing an interface that is declared to be maplike represent an ordered map of key–value pairs, initially empty, known as that object’s map entries.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-maplike">See the API definition here</see>.</remarks>
public interface IReadonlyMapLike<TMap, TKey, TValue> : IJSWrapper where TMap : IReadonlyMapLike<TMap, TKey, TValue> { }

public static class IReadonlyMapLikeExtensions
{
    public static async Task<ulong> GetSizeAsync<TMap, TKey, TValue>(this IReadonlyMapLike<TMap, TKey, TValue> map) where TMap : IReadonlyMapLike<TMap, TKey, TValue>
    {
        IJSObjectReference helper = await map.JSRuntime.GetHelperAsync();
        return await helper.InvokeAsync<ulong>("getAttribute", map.JSReference, "size");
    }
}
