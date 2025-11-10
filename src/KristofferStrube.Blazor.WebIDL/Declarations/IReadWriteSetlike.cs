using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An interface that can be used to declare an <see cref="IJSWrapper"/> as being a set that can be read from and written to.
/// </summary>
/// <typeparam name="TSet">The concrete set type.</typeparam>
/// <typeparam name="TElement">The element type that the set contains.</typeparam>
public interface IReadWriteSetlike<TSet, TElement> : IReadonlySetlike<TSet, TElement> where TSet : IReadWriteSetlike<TSet, TElement>
{
}

/// <summary>
/// Extensions for <see cref="IReadWriteSetlike{TSet, TElement}"/> types.
/// </summary>
public static class IReadWriteSetLikeExtensions
{
    /// <summary>
    /// Removes all elements from the set.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set that should be cleared.</param>
    public static async Task ClearAsync<TSet, TElement>(this IReadWriteSetlike<TSet, TElement> set) where TSet : IReadWriteSetlike<TSet, TElement>
    {
        await set.JSReference.InvokeVoidAsync("clear");
    }

    /// <summary>
    /// Inserts the element into the set.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set.</param>
    /// <param name="element">The element that will be added.</param>
    public static async Task AddAsync<TSet, TElement>(this IReadWriteSetlike<TSet, TElement> set, TElement element) where TSet : IReadWriteSetlike<TSet, TElement>
    {
        await set.JSReference.InvokeVoidAsync("add", element);
    }

    /// <summary>
    /// Removes the element from the set, if the element is already in the set.
    /// </summary>
    /// <typeparam name="TSet">The type of the set.</typeparam>
    /// <typeparam name="TElement">The type of the elements that the set contains.</typeparam>
    /// <param name="set">The set.</param>
    /// <param name="element">The element that should be removed.</param>
    /// <returns><see langword="true"/> if it was in the set; else <see langword="false"/> if it was not in the set.</returns>
    public static async Task<bool> DeleteAsync<TSet, TElement>(this IReadWriteSetlike<TSet, TElement> set, TElement element) where TSet : IReadWriteSetlike<TSet, TElement>
    {
        return await set.JSReference.InvokeAsync<bool>("delete", element);
    }
}
