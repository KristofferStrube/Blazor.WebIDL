using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A in-process version of some <see cref="TypedArray{TElement, TTypedArrayType}"/>.
/// </summary>
/// <typeparam name="TElement">The element type that the <see cref="TypedArray{TElement, TTypedArrayType}"/> contains</typeparam>
/// <typeparam name="TTypedArrayTypeInProcess">The concrete in-process type of <see cref="TypedArray{TElement, TTypedArrayType}"/></typeparam>
/// <typeparam name="TTypedArrayType">The concrete non-in-process type of <see cref="TypedArray{TElement, TTypedArrayType}"/> which <typeparamref name="TTypedArrayTypeInProcess"/> extends</typeparam>
public interface ITypedArrayInProcess<TElement, TTypedArrayTypeInProcess, TTypedArrayType>
    : IArrayBufferView, IJSWrapper, IJSInProcessCreatable<TTypedArrayTypeInProcess, TTypedArrayType>
    where TTypedArrayTypeInProcess : ITypedArrayInProcess<TElement, TTypedArrayTypeInProcess, TTypedArrayType>
    where TTypedArrayType : IJSCreatable<TTypedArrayType>
{
    /// <summary>
    /// A lazily loaded task that evaluates to a helper module instance from the Blazor.WebIDL library.
    /// </summary>
    protected IJSInProcessObjectReference InProcessHelper { get; }

    /// <summary>
    /// Creates a new empty <typeparamref name="TTypedArrayTypeInProcess"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayTypeInProcess"/>.</returns>
    public static abstract Task<TTypedArrayTypeInProcess> CreateAsync(IJSRuntime jSRuntime);

    internal static async Task<TTypedArrayTypeInProcess> CreateInternalAsync(IJSRuntime jSRuntime)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSInProcessObjectReference jSInstance = await helper.InvokeAsync<IJSInProcessObjectReference>($"construct{typeof(TTypedArrayType).Name}");
        return await TTypedArrayTypeInProcess.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <typeparamref name="TTypedArrayTypeInProcess"/> from an existing <see cref="TypedArray{TElement, TTypedArrayType}"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="typedArray">The <see cref="TypedArray{TElement, TTypedArrayType}"/> to create a new <typeparamref name="TTypedArrayTypeInProcess"/> from.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayTypeInProcess"/>.</returns>
    public static abstract Task<TTypedArrayTypeInProcess> CreateAsync<TFromElement, TFromTypedArray>(IJSRuntime jSRuntime, TypedArray<TFromElement, TFromTypedArray> typedArray) where TFromTypedArray : IJSCreatable<TFromTypedArray>;

    internal static async Task<TTypedArrayTypeInProcess> CreateInternalAsync<TFromElement, TFromTypedArray>(IJSRuntime jSRuntime, TypedArray<TFromElement, TFromTypedArray> typedArray) where TFromTypedArray : IJSCreatable<TFromTypedArray>
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSInProcessObjectReference jSInstance = await helper.InvokeAsync<IJSInProcessObjectReference>($"construct{typeof(TTypedArrayType).Name}", typedArray);
        return await TTypedArrayTypeInProcess.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <typeparamref name="TTypedArrayTypeInProcess"/> from an existing <see cref="IArrayBuffer"/>.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="buffer">The <see cref="ArrayBuffer"/> or <see cref="SharedArrayBuffer"/> to create a new <typeparamref name="TTypedArrayTypeInProcess"/> from.</param>
    /// <param name="byteOffset">The offset in the existing <paramref name="buffer"/> to have the new <typeparamref name="TTypedArrayTypeInProcess"/> start from.</param>
    /// <param name="length">The length of the new <typeparamref name="TTypedArrayTypeInProcess"/>.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayTypeInProcess"/>.</returns>
    public static abstract Task<TTypedArrayTypeInProcess> CreateAsync(IJSRuntime jSRuntime, IArrayBuffer buffer, long? byteOffset = null, long? length = null);

    internal static async Task<TTypedArrayTypeInProcess> CreateInternalAsync(IJSRuntime jSRuntime, IArrayBuffer buffer, long? byteOffset = null, long? length = null)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSInProcessObjectReference jSInstance = await helper.InvokeAsync<IJSInProcessObjectReference>($"construct{typeof(TTypedArrayType).Name}", buffer, byteOffset, length);
        return await TTypedArrayTypeInProcess.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Creates a new <typeparamref name="TTypedArrayTypeInProcess"/> with the given length.
    /// </summary>
    /// <param name="jSRuntime">An <see cref="IJSRuntime"/> instance.</param>
    /// <param name="length">Its minimum value is <c>0</c> and its maximum value is <c>2^53-1</c>.</param>
    /// <returns>A wrapper instance for a <typeparamref name="TTypedArrayTypeInProcess"/>.</returns>
    public static abstract Task<TTypedArrayTypeInProcess> CreateAsync(IJSRuntime jSRuntime, long length);

    internal static async Task<TTypedArrayTypeInProcess> CreateInternalAsync(IJSRuntime jSRuntime, long length)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSInProcessObjectReference jSInstance = await helper.InvokeAsync<IJSInProcessObjectReference>($"construct{typeof(TTypedArrayType).Name}", length);
        return await TTypedArrayTypeInProcess.CreateAsync(jSRuntime, jSInstance, new() { DisposesJSReference = true });
    }

    /// <summary>
    /// Gets the internal array buffer of the <see cref="ITypedArrayInProcess{TElement, TTypedArrayTypeInProcess, TTypedArrayType}"/>. This can either be an <see cref="ArrayBufferInProcess"/> or a <see cref="SharedArrayBufferInProcess"/>.
    /// </summary>
    public new abstract Task<IArrayBufferInProcess> GetBufferAsync();

    internal static async Task<IArrayBufferInProcess> GetBufferAsync(ITypedArrayInProcess<TElement, TTypedArrayTypeInProcess, TTypedArrayType> typedArray)
    {
        ValueReferenceInProcess bufferAttribute = await ValueReferenceInProcess.CreateAsync(typedArray.JSRuntime, typedArray.JSReference, "buffer");
        bufferAttribute.ValueMapper = new()
        {
            { "arraybuffer", async () => await bufferAttribute.GetCreatableAsync<ArrayBufferInProcess, ArrayBuffer>() },
            { "sharedarraybuffer", async () => await bufferAttribute.GetCreatableAsync<SharedArrayBufferInProcess, SharedArrayBuffer>() }
        };
        return (IArrayBufferInProcess)(await bufferAttribute.GetValueAsync())!;
    }

    /// <summary>
    /// Gets the element at the index of the array.
    /// </summary>
    /// <param name="index">The index in the array. If negative then it is interpreted at the length from the end of the array.</param>
    /// <returns>The element at the specific index.</returns>
    public abstract TElement At(long index);

    internal static TElement At(ITypedArrayInProcess<TElement, TTypedArrayTypeInProcess, TTypedArrayType> typedArray, long index)
    {
        return typedArray.JSReference.Invoke<TElement>("at", index);
    }

    /// <summary>
    /// Gets the number of elements in this array.
    /// </summary>
    /// <returns>The length as a long.</returns>
    public long Length { get; }

    internal static long GetLength(ITypedArrayInProcess<TElement, TTypedArrayTypeInProcess, TTypedArrayType> typedArray)
    {
        return typedArray.InProcessHelper.Invoke<long>("getAttribute", typedArray, "length");
    }
}
