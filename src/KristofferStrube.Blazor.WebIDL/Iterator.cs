using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class Iterator<T> : IJSWrapper<Iterator<T>>
{
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;
    public IJSObjectReference JSReference { get; }
    public IJSRuntime JSRuntime { get; }

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
}
