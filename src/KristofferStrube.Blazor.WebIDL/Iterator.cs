using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

[IJSWrapperConverter]
public class Iterator<TElement> : IJSCreatable<Iterator<TElement>>, IAsyncEnumerable<TElement>, IAsyncEnumerator<TElement> where TElement : IJSCreatable<TElement>
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

    public TElement Current { get; private set; } = default!;

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

    public Iterator(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposeOfJSReference;
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        IJSObjectReference next = await JSReference.InvokeAsync<IJSObjectReference>("next");
        IJSObjectReference helper = await helperTask.Value;
        bool done = await helper.InvokeAsync<bool>("getAttribute", next, "done");
        if (done)
        {
            Current = default!;
            return false;
        }

        CreationOptions options = new()
        {
            DisposeOfJSReference = true
        };

        if (typeof(TElement) == typeof(ValueReference))
        {
            Current = await TElement.CreateAsync(JSRuntime, next, options);
        }
        else
        {
            IJSObjectReference jSValue = await helper.InvokeAsync<IJSObjectReference>("getAttribute", next, "value");
            Current = await TElement.CreateAsync(JSRuntime, jSValue, options);
        }
        return true;
    }

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
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}

public class StructIterator<T> : IJSCreatable<StructIterator<T>>, IAsyncEnumerable<T>, IAsyncEnumerator<T> where T : struct
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public bool DisposesJSReference { get; }

    public T Current { get; private set; } = default;

    /// <inheritdoc/>
    public static async Task<StructIterator<T>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new StructIterator<T>(jSRuntime, jSReference, new()));
    }

    /// <inheritdoc/>
    public static async Task<StructIterator<T>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        return await Task.FromResult(new StructIterator<T>(jSRuntime, jSReference, options));
    }

    public StructIterator(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSReference = jSReference;
        JSRuntime = jSRuntime;
        DisposesJSReference = options.DisposeOfJSReference;
    }

    public async ValueTask<bool> MoveNextAsync()
    {
        IJSObjectReference next = await JSReference.InvokeAsync<IJSObjectReference>("next");
        IJSObjectReference helper = await helperTask.Value;
        bool done = await helper.InvokeAsync<bool>("getAttribute", next, "done");
        if (done)
        {
            Current = default;
            return false;
        }

        Current = await helper.InvokeAsync<T>("getAttribute", next, "value");
        return true;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
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
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}