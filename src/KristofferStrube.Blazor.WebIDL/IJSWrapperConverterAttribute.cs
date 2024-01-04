using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An attribute for making any <see cref="IJSWrapper"/> JSON serializable.
/// </summary>
public class IJSWrapperConverterAttribute : JsonConverterAttribute
{
    /// <summary>
    /// Creates the <see cref="IJSWrapperConverter{TWrapper}"/> for serializing a specific <see cref="IJSWrapper"/>.
    /// </summary>
    /// <param name="typeToConvert">The type to create a <see cref="IJSWrapperConverter{TWrapper}"/> for.</param>
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        if (typeToConvert.IsAssignableTo(typeof(IJSWrapper)))
        {
            return (JsonConverter)typeof(IJSWrapperConverter<>).MakeGenericType(typeToConvert).GetConstructor(Array.Empty<Type>())!.Invoke(null);
        }
        return null;
    }
}
