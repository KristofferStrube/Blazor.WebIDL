using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A TypedArray presents an array-like view of an underlying binary data buffer.
/// A TypedArray element type is the underlying binary scalar data type that all elements of a TypedArray instance have.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/indexed-collections.html#sec-typedarray-objects">See the API definition here</see>.</remarks>
/// <typeparam name="TElement">The element type.</typeparam>
/// <typeparam name="TTypedArrayType">The type of the deriving type.</typeparam>
public abstract class TypedArray<TElement, TTypedArrayType> : IArrayBufferView, IJSWrapper where TTypedArrayType : IJSCreatable<TTypedArrayType>
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
    public bool DisposesJSReference { get; }

    /// <summary>
    /// Creates a new empty <typeparamref name="TTypedArrayType"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayType"/>.</returns>
    public static async Task<TTypedArrayType> CreateAsync(IJSRuntime jSRuntime)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>($"construct{typeof(TTypedArrayType).Name}");
        return await TTypedArrayType.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <typeparamref name="TTypedArrayType"/> from an existing <see cref="TypedArray{TElement, TTypedArrayType}"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="typedArray">The <see cref="TypedArray{TElement, TTypedArrayType}"/> to create a new <typeparamref name="TTypedArrayType"/> from.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayType"/>.</returns>
    public static async Task<TTypedArrayType> CreateAsync<TFromElement, TFromTypedArray>(IJSRuntime jSRuntime, TypedArray<TFromElement, TFromTypedArray> typedArray) where TFromTypedArray : IJSCreatable<TFromTypedArray>
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>($"construct{typeof(TTypedArrayType).Name}", typedArray);
        return await TTypedArrayType.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <typeparamref name="TTypedArrayType"/> from an existing <see cref="IArrayBuffer"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="buffer">The <see cref="ArrayBuffer"/> or <see cref="SharedArrayBuffer"/> to create a new <typeparamref name="TTypedArrayType"/> from.</param>
    /// <param name="byteOffset">The offset in the existing <paramref name="buffer"/> to have the new <typeparamref name="TTypedArrayType"/> start from.</param>
    /// <param name="length">The length of the new <typeparamref name="TTypedArrayType"/>.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayType"/>.</returns>
    public static async Task<TTypedArrayType> CreateAsync(IJSRuntime jSRuntime, IArrayBuffer buffer, long? byteOffset = null, long? length = null)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>($"construct{typeof(TTypedArrayType).Name}", buffer, byteOffset, length);
        return await TTypedArrayType.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <typeparamref name="TTypedArrayType"/> with the given length.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="length">Its minimum value is <c>0</c> and its maximum value is <c>2^53-1</c>.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayType"/>.</returns>
    public static async Task<TTypedArrayType> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>($"construct{typeof(TTypedArrayType).Name}", length);
        return await TTypedArrayType.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Constructs a wrapper instance for a given JS Instance of a <see cref="TypedArray{TElement, TTypedArrayType}"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="jSReference">A JS reference to an existing <see cref="TypedArray{TElement, TTypedArrayType}"/>.</param>
    /// <param name="options">The options for constructing this wrapper</param>
    protected TypedArray(IJSRuntime jSRuntime, IJSObjectReference jSReference, CreationOptions options)
    {
        helperTask = new(jSRuntime.GetHelperAsync);
        JSRuntime = jSRuntime;
        JSReference = jSReference;
        DisposesJSReference = options.DisposesJSReference;
    }

    /// <summary>
    /// Gets the internal array buffer of the <see cref="TypedArray{TElement, TTypedArrayType}"/>. This can either be an <see cref="ArrayBuffer"/> or a <see cref="SharedArrayBuffer"/>.
    /// </summary>
    public async Task<IArrayBuffer> GetBufferAsync()
    {
        ValueReference bufferAttribute = new(JSRuntime, JSReference, "buffer");
        bufferAttribute.ValueMapper = new()
        {
            { "arraybuffer", async () => await bufferAttribute.GetCreatableAsync<ArrayBuffer>() },
            { "sharedarraybuffer", async () => await bufferAttribute.GetCreatableAsync<SharedArrayBuffer>() }
        };
        return (IArrayBuffer)(await bufferAttribute.GetValueAsync())!;
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
    public async Task<TElement> AtAsync<TCreatableElement>(long index)
        where TCreatableElement : IJSCreatable<TCreatableElement>, TElement
    {
        IJSObjectReference jSInstance = await JSReference.InvokeAsync<IJSObjectReference>("at", index);
        CreationOptions options = new()
        {
            DisposesJSReference = true
        };
        return await TCreatableElement.CreateAsync(JSRuntime, jSInstance, options);
    }

    /// <summary>
    /// Fills the array with <paramref name="value"/> from the <paramref name="start"/> to the <paramref name="end"/>.
    /// </summary>
    /// <param name="value">The value that the array will be filled with.</param>
    /// <param name="start">The start index from which the array will be filled. If the number is negative then it will index from the end of the array. It defaults to the start of the array.</param>
    /// <param name="end">The end index to which the array will be filled. The end index is not inclusive. If the number is negative then it will index from the end of the array. It defaults to the end of the array.</param>
    public async Task FillAsync(TElement value, long? start = null, long? end = null)
    {
        if (start is null)
        {
            await JSReference.InvokeVoidAsync("fill", value);
        }
        else if (end is null)
        {
            await JSReference.InvokeVoidAsync("fill", value, start);
        }
        else
        {
            await JSReference.InvokeVoidAsync("fill", value, start, end);
        }
    }

    /// <summary>
    /// Gets the number of elements in this array.
    /// </summary>
    /// <returns>The length as a long.</returns>
    public async Task<long> GetLengthAsync()
    {
        IJSObjectReference helper = await helperTask.Value;
        return await helper.InvokeAsync<long>("getAttribute", this, "length");
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (helperTask.IsValueCreated)
        {
            IJSObjectReference module = await helperTask.Value;
            await module.DisposeAsync();
        }
        await IJSWrapper.DisposeJSReference(this);
        GC.SuppressFinalize(this);
    }
}