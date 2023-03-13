using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IJSWrapper
{
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }
}