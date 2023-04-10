using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

public class Float32Array : TypedArray<float>, IJSCreatable<Float32Array>
{
    public static Task<Float32Array> CreateAsync(IJSRuntime jSRuntime, IJSObjectReference jSReference)
    {
        return Task.FromResult(new Float32Array(jSRuntime, jSReference));
    }

    /// <summary>
    /// Creates a new <see cref="Float32Array"/> with the given length.
    /// </summary>
    /// <param name="jSRuntime"></param>
    /// <param name="length">Its minimum value is <c>0</c> and its maximum value is <c>2^53-1</c>.</param>
    /// <returns>The new array.</returns>
    public static async Task<Float32Array> CreateAsync(IJSRuntime jSRuntime, long length)
    {
        var helper = await jSRuntime.GetHelperAsync();
        var jSInstance = await helper.InvokeAsync<IJSObjectReference>("constructFloat32Array", length);
        return new Float32Array(jSRuntime, jSInstance);
    }

    public Float32Array(IJSRuntime jSRuntime, IJSObjectReference jSReference) : base(jSRuntime, jSReference) { }
}
