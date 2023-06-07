using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

internal sealed class JSObjectReferenceJsonConverter : JsonConverter<IJSObjectReference>
{
    private readonly JSRuntime _jsRuntime;

    public JSObjectReferenceJsonConverter(JSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(IJSObjectReference) || typeToConvert == typeof(JSObjectReference);
    }

    public override IJSObjectReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long id = JSObjectReferenceJsonWorker.ReadJSObjectReferenceIdentifier(ref reader);
        return new ConstructableJSObjectReference(_jsRuntime, id);
    }

    public override void Write(Utf8JsonWriter writer, IJSObjectReference value, JsonSerializerOptions options)
    {
        JSObjectReferenceJsonWorker.WriteJSObjectReference(writer, (JSObjectReference)value);
    }
}
