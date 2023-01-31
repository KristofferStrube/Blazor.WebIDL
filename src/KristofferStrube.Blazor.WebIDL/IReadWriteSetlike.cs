namespace KristofferStrube.Blazor.WebIDL;

public interface IReadWriteSetlike<T> : IReadonlySetlike<T> where T : IJSWrapper
{
    public async Task<T> AddAsync(T element)
    {
        return await Task.FromResult<T>(default!);
    }

    public async Task<bool> ClearAsync()
    {
        return await Task.FromResult<bool>(default!);
    }

    public async Task<bool> DeleteAsync(T element)
    {
        return await Task.FromResult<bool>(default!);
    }
}
