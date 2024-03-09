namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A common interface for objects that provide a view on to an <see cref="ArrayBuffer"/> or <see cref="SharedArrayBuffer"/>.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/structured-data.html#sec-arraybuffer-objects">See the API definition here</see>.</remarks>
public interface IArrayBufferView : IJSWrapper
{
    /// <summary>
    /// Gets the internal array buffer. This can either be an <see cref="ArrayBuffer"/> or a <see cref="SharedArrayBuffer"/>.
    /// </summary>
    /// <returns></returns>
    public Task<IArrayBuffer> GetBufferAsync();
}
