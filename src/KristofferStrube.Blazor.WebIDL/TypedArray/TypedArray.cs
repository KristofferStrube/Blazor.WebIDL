using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public abstract class TypedArray<TElement> : IJSWrapper
{
    public IJSRuntime JSRuntime { get; }
    public IJSObjectReference JSReference { get; }

    public TypedArray(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        JSRuntime = jSRuntime;
        JSReference = jSReference;
    }

    /// <summary>
    /// Gets the element at the index of the array.
    /// </summary>
    /// <param name="index">The index in the array. If negative then it is interpreted at the length from the end of the array.</param>
    /// <returns>The element at the specific index.</returns>
    public async Task<TElement> AtAsync(long index)
    {
        return await JSReference.InvokeAsync<TElement>("at", index);
    }
}