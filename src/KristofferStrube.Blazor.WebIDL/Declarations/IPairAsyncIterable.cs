using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Types that implement <see cref="IPairAsyncIterable{TAsyncIterable, TKey, TValue}"/> support being iterated over asynchronously to obtain a sequence of pairs or the pairs keys and values individually.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-async-iterable-declaration">See the API definition here</see>.</remarks>
/// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
/// <typeparam name="TKey">The type of the keys that can be iterated.</typeparam>
/// <typeparam name="TValue">The type of the values that can be iterated.</typeparam>
public interface IPairAsyncIterable<TAsyncIterable, TKey, TValue> : IValueAsyncIterable<TAsyncIterable, TValue>, IJSWrapper where TAsyncIterable : IPairAsyncIterable<TAsyncIterable, TKey, TValue>
{
}

/// <summary>
/// Extensions used to access members of objects that implement <see cref="IPairAsyncIterable{TAsyncIterable, TKey, TValue}"/>.
/// </summary>
public static class PairAsyncIterableExtensions
{
    /// <summary>
    /// Gets an async iterator for the entries of the <paramref name="asyncIterable"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each element when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
    /// <typeparam name="TKey">The type of the keys in the async iterable.</typeparam>
    /// <typeparam name="TValue">The type of the values in the async iterable.</typeparam>
    /// <param name="asyncIterable">The object to iterate.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value when the iterator moves on to the next.</param>
    public static async Task<AsyncIterator<TKey, TValue>> EntriesAsync<TAsyncIterable, TKey, TValue>(this IPairAsyncIterable<TAsyncIterable, TKey, TValue> asyncIterable, bool disposePreviousValueWhenMovingToNextValue = true) where TAsyncIterable : IPairAsyncIterable<TAsyncIterable, TKey, TValue>
    {
        IJSObjectReference jSEntriesIterator = await asyncIterable.JSReference.InvokeAsync<IJSObjectReference>("entries");
        AsyncIterator<TKey, TValue> iterator = await AsyncIterator<TKey, TValue>.CreateAsync(asyncIterable.JSRuntime, jSEntriesIterator, new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Gets an async iterator for the keys of the <paramref name="asyncIterable"/>.
    /// When <paramref name="disposePreviousKeyWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each element when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
    /// <typeparam name="TKey">The type of the keys in the async iterable.</typeparam>
    /// <typeparam name="TValue">The type of the values in the async iterable.</typeparam>
    /// <param name="asyncIterable">The object to iterate.</param>
    /// <param name="disposePreviousKeyWhenMovingToNextValue">Whether it should dispose the prior value when the iterator moves on to the next.</param>
    public static async Task<AsyncIterator<TKey>> KeysAsync<TAsyncIterable, TKey, TValue>(this IPairAsyncIterable<TAsyncIterable, TKey, TValue> asyncIterable, bool disposePreviousKeyWhenMovingToNextValue = true) where TAsyncIterable : IPairAsyncIterable<TAsyncIterable, TKey, TValue>
    {
        IJSObjectReference jSKeysIterator = await asyncIterable.JSReference.InvokeAsync<IJSObjectReference>("keys");
        AsyncIterator<TKey> iterator = await AsyncIterator<TKey>.CreateAsync(asyncIterable.JSRuntime, jSKeysIterator, new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousKeyWhenMovingToNextValue;
        return iterator;
    }
}