using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IJSWrapper<T> : IJSWrapper where T : IJSWrapper<T>
{
    public static abstract Task<T> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference);
}
public interface IJSWrapper
{
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }
}