using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IReadWriteSetlike<TSet> : IReadonlySetlike<TSet> where TSet : IReadWriteSetlike<TSet> { }

public static class IReadWriteSetlikeExtensions
{
    public static async Task<TSet> AddAsync<TSet, T>(this TSet set, T element) where TSet : IReadWriteSetlike<TSet> where T : IJSWrapper
    {
        await set.JSReference.InvokeVoidAsync("add", element);
        return set;
    }
    public static async Task ClearAsync<TSet>(this TSet set) where TSet : IReadWriteSetlike<TSet>
    {
        await set.JSReference.InvokeVoidAsync("clear");
    }
    public static async Task<bool> DeleteAsync<TSet, T>(this TSet set, T element) where TSet : IReadWriteSetlike<TSet> where T : IJSWrapper
    {
        return await set.JSReference.InvokeAsync<bool>("delete", element);
    }
}

public static class IReadWriteSetlikeStructExtensions
{
    public static async Task<TSet> AddAsync<TSet, T>(this TSet set, T element) where TSet : IReadWriteSetlike<TSet> where T : struct
    {
        await set.JSReference.InvokeVoidAsync("add", element);
        return set;
    }
    public static async Task<bool> DeleteAsync<TSet, T>(this TSet set, T element) where TSet : IReadWriteSetlike<TSet> where T : struct
    {
        return await set.JSReference.InvokeAsync<bool>("delete", element);
    }
}

public static class IReadWriteSetlikeObjectExtensions
{
    public static async Task<TSet> AddAsync<TSet, T>(this TSet set, T element) where TSet : IReadWriteSetlike<TSet> where T : class
    {
        await set.JSReference.InvokeVoidAsync("add", element);
        return set;
    }
    public static async Task<bool> DeleteAsync<TSet, T>(this TSet set, T element) where TSet : IReadWriteSetlike<TSet> where T : class
    {
        return await set.JSReference.InvokeAsync<bool>("delete", element);
    }
}