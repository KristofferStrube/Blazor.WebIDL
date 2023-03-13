using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class Iterator<T> : IJSCreatable<Iterator<T>>, IAsyncEnumerable<T>, IAsyncEnumerator<T> where T : IJSCreatable<T>
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

    public T Current { get; private set; } = default!;

    public static async Task<Iterator<T>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new Iterator<T>(jSRuntime, jSReference));
    }

    public Iterator(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
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
        if (typeof(T) == typeof(ValueReference))
        {
            Current = await T.CreateAsync(JSRuntime, next);
        }
        else
        {
            IJSObjectReference jSValue = await helper.InvokeAsync<IJSObjectReference>("getAttribute", next, "value");
            Current = await T.CreateAsync(JSRuntime, jSValue);
        }
        return true;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return this;
    }

    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}

public class StructIterator<T> : IJSCreatable<StructIterator<T>>, IAsyncEnumerable<T>, IAsyncEnumerator<T> where T : struct
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

    public T Current { get; private set; } = default;

    public static async Task<StructIterator<T>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return await Task.FromResult(new StructIterator<T>(jSRuntime, jSReference));
    }

    public StructIterator(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
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

    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        GC.SuppressFinalize(this);
    }
}