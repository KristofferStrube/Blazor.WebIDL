using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Types that implement <see cref="IValueAsyncIterable{TAsyncIterable, TValue}"/> support being iterated over asynchronously to obtain a sequence of values.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-async-iterable-declaration">See the API definition here</see>.</remarks>
/// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
/// <typeparam name="TValue">The type of the values that can be iterated.</typeparam>
public interface IValueAsyncIterable<TAsyncIterable, TValue> : IJSWrapper where TAsyncIterable : IValueAsyncIterable<TAsyncIterable, TValue>
{
}

/// <summary>
/// Types that implement <see cref="IValueAsyncIterable{TAsyncIterable, TValue, TIteratorOptions}"/> support being iterated over asynchronously to obtain a sequence of values.
/// </summary>
/// <remarks><see href="https://webidl.spec.whatwg.org/#idl-async-iterable-declaration">See the API definition here</see>.</remarks>
/// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
/// <typeparam name="TValue">The type of the values that can be iterated.</typeparam>
/// <typeparam name="TIteratorOptions">The type of the options that can be used to adjust how the async iterable will be iterated.</typeparam>
public interface IValueAsyncIterable<TAsyncIterable, TValue, TIteratorOptions> : IJSWrapper where TAsyncIterable : IValueAsyncIterable<TAsyncIterable, TValue, TIteratorOptions>
{
}

/// <summary>
/// Extensions used to access members of objects that implement <see cref="IValueAsyncIterable{TAsyncIterable, TValue}"/> or <see cref="IValueAsyncIterable{TAsyncIterable, TValue, TIteratorOptions}"/>.
/// </summary>
public static class ValueAsyncIterableExtensions
{
    /// <summary>
    /// Gets an async iterator for the values in the <paramref name="asyncIterable"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each element when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
    /// <typeparam name="TValue">The type of the values in the async iterable.</typeparam>
    /// <param name="asyncIterable">The object to iterate.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value when the iterator moves on to the next.</param>
    public static async Task<AsyncIterator<TValue>> ValuesAsync<TAsyncIterable, TValue>(this IValueAsyncIterable<TAsyncIterable, TValue> asyncIterable, bool disposePreviousValueWhenMovingToNextValue = true) where TAsyncIterable : IValueAsyncIterable<TAsyncIterable, TValue>
    {
        IJSObjectReference jSValuesIterator = await asyncIterable.JSReference.InvokeAsync<IJSObjectReference>("values");
        AsyncIterator<TValue> iterator = await AsyncIterator<TValue>.CreateAsync(asyncIterable.JSRuntime, jSValuesIterator, new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }

    /// <summary>
    /// Gets an async iterator for the values in the <paramref name="asyncIterable"/>.
    /// When <paramref name="disposePreviousValueWhenMovingToNextValue"/> is set to <see langword="true"/>; it will dispose each element when the iterator moves to the next or completes.
    /// </summary>
    /// <typeparam name="TAsyncIterable">The type of the async iterable.</typeparam>
    /// <typeparam name="TValue">The type of the values in the async iterable.</typeparam>
    /// <typeparam name="TIteratorOptions">The type of the options that can be used to adjust how the async iterable will be iterated.</typeparam>
    /// <param name="asyncIterable">The object to iterate.</param>
    /// <param name="options">Options for adjusting how the object will iterated.</param>
    /// <param name="disposePreviousValueWhenMovingToNextValue">Whether it should dispose the prior value when the iterator moves on to the next.</param>
    public static async Task<AsyncIterator<TValue>> ValuesAsync<TAsyncIterable, TValue, TIteratorOptions>(this IValueAsyncIterable<TAsyncIterable, TValue, TIteratorOptions> asyncIterable, TIteratorOptions? options = default, bool disposePreviousValueWhenMovingToNextValue = true) where TAsyncIterable : IValueAsyncIterable<TAsyncIterable, TValue, TIteratorOptions>
    {
        IJSObjectReference jSValuesIterator = await asyncIterable.JSReference.InvokeAsync<IJSObjectReference>("values", options);
        AsyncIterator<TValue> iterator = await AsyncIterator<TValue>.CreateAsync(asyncIterable.JSRuntime, jSValuesIterator, new() { DisposesJSReference = true });
        iterator.DisposePreviousValueWhenMovingToNextValue = disposePreviousValueWhenMovingToNextValue;
        return iterator;
    }
}