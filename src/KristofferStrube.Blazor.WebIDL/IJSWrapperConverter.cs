using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// A JSON converter that can correctly serialize a class that implements the <see cref="IJSWrapper"/> interface.
/// </summary>
public class IJSWrapperConverter<TWrapper> : JsonConverter<TWrapper> where TWrapper : IJSWrapper
{
    /// <summary>
    /// Deserialization is not supported.
    /// </summary>
    /// <exception cref="NotSupportedException"></exception>
    public override TWrapper Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException($"Deserialization of a {nameof(IJSWrapper)} is not supported directly from JSON.");
    }

    /// <summary>
    /// Serializes the class that implements the <see cref="IJSWrapper"/> interface.
    /// </summary>
    public override void Write(Utf8JsonWriter writer, TWrapper value, JsonSerializerOptions options)
    {
        IJSObjectReference jsReference = value.JSReference;

        if (jsReference is IErrorHandlingJSObjectReference errorHandlingJSObjectReference)
        {
            jsReference = errorHandlingJSObjectReference.JSReference;
        }

        writer.WriteRawValue(JsonSerializer.Serialize(jsReference, options));
    }
}
