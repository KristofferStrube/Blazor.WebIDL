using System.Reflection;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// 
/// </summary>
[AttributeUsage(validOn: AttributeTargets.Class, Inherited = true)]
public class IJSWrapperConverterAttribute : JsonConverterAttribute
{
    public override JsonConverter? CreateConverter(Type typeToConvert)
    {
        if (typeToConvert.IsAssignableTo(typeof(IJSWrapper)))
        {
            return (JsonConverter)typeof(IJSWrapperConverter<>).MakeGenericType(typeToConvert).GetConstructor(Array.Empty<Type>())!.Invoke(null);
        }
        return null;
    }
}
