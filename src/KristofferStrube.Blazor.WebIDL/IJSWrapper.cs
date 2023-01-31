using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public interface IJSWrapper
{
    public IJSObjectReference JSReference { get; }
    protected IJSObjectReference JSRuntime { get; }
}
