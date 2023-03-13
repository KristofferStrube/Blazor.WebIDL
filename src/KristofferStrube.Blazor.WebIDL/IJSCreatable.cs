using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IJSCreatable<T> : IJSWrapper where T : IJSCreatable<T>
{
    public static abstract Task<T> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference);
}