using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A TypedArray presents an array-like view of an underlying binary data buffer. A TypedArray element type is the underlying binary scalar data type that all elements of a TypedArray instance have.
/// </summary>
/// <typeparam name="TElement"></typeparam>
public class TypedArray<TElement> : IJSCreatable<TypedArray<TElement>>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected readonly Lazy<Task<IJSObjectReference>> helperTask;

    /// <inheritdoc/>
    public IJSRuntime JSRuntime { get; }

    /// <inheritdoc/>
    public IJSObjectReference JSReference { get; }

    /// <inheritdoc/>
    public static Task<TypedArray<TElement>> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new TypedArray<TElement>(jSRuntime, jSReference));
    }

    /// <summary>
    /// Constructs a new <see cref="TypedArray{TElement}"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing JS instance that should be wrapped.</param>
    public TypedArray(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        helperTask = new(() => jSRuntime.GetHelperAsync());
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

    /// <summary>
    /// Gets the element at the index of the array as a wrapper instance.
    /// </summary>
    /// <param name="index">The index in the array. If negative then it is interpreted at the length from the end of the array.</param>
    /// <returns>The element at the specific index.</returns>
    public async Task<TElement> AtAsync<TCreatableElement>(long index) where TCreatableElement : IJSCreatable<TCreatableElement>, TElement
    {
        var jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("at", index);
        return await TCreatableElement.CreateAsync(JSRuntime, jSInstance);
    }

    /// <summary>
    /// Gets the number of elements in this array.
    /// </summary>
    /// <returns>The length as a long.</returns>
    public async Task<long> GetLengthAsync()
    {
        var helper = await helperTask.Value;
        return await helper.InvokeAsync<long>("getAttribute", JSReference, "length");
    }
}