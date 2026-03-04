using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A wrapper for a JS async iterator that can also be iterated as an <see cref="IAsyncEnumerator{T}"/>.
/// </summary>
/// <remarks>
/// It is also <see cref="IAsyncEnumerable{T}"/>, but it will return the same <see cref="IAsyncEnumerator{T}"/> every time it is iterated so you will have to create a new iterator if you want to iterate the underlying collection multiple times.
/// </remarks>
/// <typeparam name="TElement">The type of the element that is iterated.</typeparam>
[IJSWrapperConverter]
public class AsyncIterator<TElement> : Iterator<TElement>, IJSCreatable<AsyncIterator<TElement>>
{
    /// <inheritdoc/>
    public static new async Task<AsyncIterator<TElement>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new AsyncIterator<TElement>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static new async Task<AsyncIterator<TElement>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new AsyncIterator<TElement>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected AsyncIterator(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options)
    {
    }

    /// <summary>
    /// Cancels the iterator.
    /// </summary>
    /// <returns></returns>
    public async Task ReturnAsync()
    {
        await JSReference.InvokeVoidAsync("return");
    }
}

/// <summary>
/// A wrapper for a JS iterator that can also be iterated as an <see cref="IAsyncEnumerator{T}"/>.
/// It iterates over key-value-pairs i.e. <see cref="KeyValuePair{TKey, TValue}"/>
/// </summary>
/// <remarks>
/// It is also <see cref="IAsyncEnumerable{T}"/>, but it will return the same <see cref="IAsyncEnumerator{T}"/> every time it is iterated so you will have to create a new iterator if you want to iterate the underlying collection multiple times.
/// </remarks>
/// <typeparam name="TKey">The key type of the pairs that are iterated.</typeparam>
/// <typeparam name="TValue">The value type of the pairs that are iterated.</typeparam>
[IJSWrapperConverter]
public class AsyncIterator<TKey, TValue> : Iterator<TKey, TValue>, IJSCreatable<AsyncIterator<TKey, TValue>>
{
    /// <inheritdoc/>
    public static new async Task<AsyncIterator<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new AsyncIterator<TKey, TValue>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static new async Task<AsyncIterator<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new AsyncIterator<TKey, TValue>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected AsyncIterator(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options) : base(jSRuntime, jSReference, options) { }

    /// <summary>
    /// Cancels the iterator.
    /// </summary>
    /// <returns></returns>
    public async Task ReturnAsync()
    {
        await JSReference.InvokeVoidAsync("return");
    }
}