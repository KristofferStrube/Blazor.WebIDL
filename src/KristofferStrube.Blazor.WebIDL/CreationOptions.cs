namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// The standard option type used to construct a <see cref="IJSCreatable{T}"/>.
/// </summary>
public class CreationOptions
{
    /// <summary>
    /// Whether the constructed <see cref="IJSCreatable{T}"/> should dispose its <see cref="IJSWrapper.JSReference"/> when it is disposed.
    /// </summary>
    /// <remarks>
    /// The default is <see langword="false"/>.
    /// </remarks>
    public bool DisposeOfJSReference { get; set; } = false;
}