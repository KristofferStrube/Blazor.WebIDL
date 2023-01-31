using System.Collections.ObjectModel;

namespace KristofferStrube.Blazor.WebIDL;

public interface IReadonlySetlike<T> : IJSWrapper, IAsyncEnumerable<T> where T : IJSWrapper
{
    public async Task<IDictionary<string, T>> EntriesAsync()
    {
        return await Task.FromResult<IDictionary<string, T>>(default!);
    }

    public async Task ForEachAsync(Func<T, Task> action)
    {
        await Task.CompletedTask;
    }

    public async Task<bool> HasAsync(T element)
    {
        return await Task.FromResult<bool>(default!);
    }

    public async Task<ReadOnlyCollection<T>> ValuesAsync()
    {
        return await Task.FromResult<ReadOnlyCollection<T>>(default!);
    }

    public async Task<IAsyncEnumerator<T>> IteratorAsync()
    {
        return await Task.FromResult<IAsyncEnumerator<T>>(default!);
    }

    public async Task<ulong> GetSizeAsync()
    {
        return await Task.FromResult<ulong>(default!);
    }
}
