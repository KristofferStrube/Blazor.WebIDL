using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class Uint8Array : TypedArray<byte>, IJSCreatable<Uint8Array>
{
    public static Task<Uint8Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Uint8Array(jSRuntime, jSReference));
    }

    /// <summary>
    /// Creates a new <see cref="Uint8Array"/> with the given length.
    /// </summary>
    /// <param name="jSRuntime"></param>
    /// <param name="length">Its minimum value is <c>0</c> and its maximum value is <c>2^53-1</c>.</param>
    /// <returns>The new array.</returns>
    public static async Task<Uint8Array> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        IJSObjectReference helper = await jSRuntime.GetHelperAsync();
        IJSObjectReference jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructUint8Array", length);
        return new Uint8Array(jSRuntime, jSInstance);
    }

    public Uint8Array(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }

    /// <summary>
    /// Helper method for converting this to a byteArray,
    /// </summary>
    /// <returns>The corresponding byte array that this <see cref="Uint8Array"/> represents.</returns>
    public async Task<byte[]> GetByteArrayAsync()
    {
        return await JSReference.InvokeAsync<byte[]>("valueOf");
    }

    /// <summary>
    /// Helper method for converting this to a byteArray,
    /// </summary>
    /// <returns>The corresponding byte array that this <see cref="Uint8Array"/> represents.</returns>
    public static explicit operator Task<byte[]>(Uint8Array wrappedArray)
    {
        return wrappedArray.GetByteArrayAsync();
    }
}
