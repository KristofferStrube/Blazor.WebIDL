using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A wrapper for a JS iterator that can also be iterated as an <see cref="IAsyncEnumerator{T}"/>.
/// </summary>
/// <remarks>
/// It is also <see cref="IAsyncEnumerable{T}"/>, but it will return the same <see cref="IAsyncEnumerator{T}"/> every time it is iterated so you will have to create a new iterator if you want to iterate the underlying collection multiple times.
/// </remarks>
/// <typeparam name="TElement"></typeparam>
[IJSWrapperConverter]
public class Iterator<TElement> : IJSCreatable<Iterator<TElement>>, IAsyncEnumerable<TElement>, IAsyncEnumerator<TElement>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <summary>
    /// The current element that the iterator is at.
    /// </summary>
    public TElement Current { get; private set; } = default!;

    /// <summary>
    /// Determines whether the iterator should dispose the values that it iterates once it moves to the next.
    /// </summary>
    public bool DisposePreviousValueWhenMovingToNextValue { get; set; }

    /// <inheritdoc/>
    public static async Task<Iterator<TElement>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Iterator<TElement>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<Iterator<TElement>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new Iterator<TElement>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    protected Iterator(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    /// <inheritdoc/>
    public async ValueTask<bool> MoveNextAsync()
    {
        if (DisposePreviousValueWhenMovingToNextValue && Current is IAsyncDisposable { } disposable)
        {
            await disposable.DisposeAsync();
        }

        await using IJSObjectReference next = await JSReference.InvokeAsync<IJSObjectReference>("next");
        IJSObjectReference helper = await helperTask.Value;
        bool done = await helper.InvokeAsync<bool>("getAttribute", next, "done");
        if (done)
        {
            Current = default!;
            return false;
        }

        Current = await IteratorValueHydrator.GetConstructedValue<TElement, string>(JSRuntime, helper, next, "value");
        return true;
    }

    /// <summary>
    /// Returns this object.
    /// </summary>
    /// <returns></returns>
    public IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return this;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        if (DisposePreviousValueWhenMovingToNextValue && Current is IAsyncDisposable { } disposable)
        {
            await disposable.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
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
public class Iterator<TKey, TValue> : IJSCreatable<Iterator<TKey, TValue>>, IAsyncEnumerable<KeyValuePair<TKey, TValue>>, IAsyncEnumerator<KeyValuePair<TKey, TValue>>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    /// <summary>
    /// The current element that the iterator is at.
    /// </summary>
    public KeyValuePair<TKey, TValue> Current { get; private set; } = default!;

    /// <summary>
    /// Determines whether the iterator should dispose the values that it iterates once it moves to the next.
    /// </summary>
    public bool DisposePreviousValueWhenMovingToNextValue { get; set; }

    /// <inheritdoc/>
    public static async Task<Iterator<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Iterator<TKey, TValue>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<Iterator<TKey, TValue>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new Iterator<TKey, TValue>(jSRuntime, jSReference, options));
    }

    /// <inheritdoc cref="CreateAsync(IJSRuntime, IJSObjectReference, CreationOptions)"/>
    public Iterator(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    /// <inheritdoc/>
    public async ValueTask<bool> MoveNextAsync()
    {
        if (DisposePreviousValueWhenMovingToNextValue)
        {
            if (Current.Key is IAsyncDisposable { } disposableKey)
            {
                await disposableKey.DisposeAsync();
            }
            if (Current.Value is IAsyncDisposable { } dispoableValue)
            {
                await dispoableValue.DisposeAsync();
            }
        }

        await using IJSObjectReference next = await JSReference.InvokeAsync<IJSObjectReference>("next");
        IJSObjectReference helper = await helperTask.Value;
        bool done = await helper.InvokeAsync<bool>("getAttribute", next, "done");
        if (done)
        {
            Current = default!;
            return false;
        }

        await using IJSObjectReference pair = await helper.InvokeAsync<IJSObjectReference>("getAttribute", next, "value");

        TKey key = await IteratorValueHydrator.GetConstructedValue<TKey, int>(JSRuntime, helper, pair, 0);
        TValue value = await IteratorValueHydrator.GetConstructedValue<TValue, int>(JSRuntime, helper, pair, 1);

        Current = new KeyValuePair<TKey, TValue>(key, value);
        return true;
    }



    /// <summary>
    /// Returns this object.
    /// </summary>
    /// <returns></returns>
    public IAsyncEnumerator<KeyValuePair<TKey, TValue>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return this;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        if (DisposePreviousValueWhenMovingToNextValue && Current is IAsyncDisposable { } disposable)
        {
            await disposable.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}

file static class IteratorValueHydrator
{
    public static async Task<T> GetConstructedValue<T, TAttributeType>(IJSRuntime jSRuntime, IJSObjectReference helper, IJSObjectReference holder, TAttributeType attributeName)
    {
        if (typeof(T).GetInterfaces().Any(i => i.IsConstructedGenericType && i.GetGenericTypeDefinition() == typeof(IJSCreatable<>)))
        {
            IJSObjectReference jSValue = await helper.InvokeAsync<IJSObjectReference>("getAttribute", holder, attributeName);
            return await DeclarationJSMapping.CallCreateAsync<T>(jSValue, jSRuntime);
        }
        else
        {
            return await helper.InvokeAsync<T>("getAttribute", holder, attributeName);
        }
    }
}