﻿using Microsoft.JSInterop.Implementation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

internal sealed class JSObjectReferenceJsonConverter : JsonConverter<IErrorHandlingJSObjectReference>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ErrorHandlingJSInProcessObjectReference) || typeToConvert == typeof(ErrorHandlingJSObjectReference);
    }

    public override IErrorHandlingJSObjectReference? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotSupportedException($"Deserialization of a {nameof(IErrorHandlingJSObjectReference)} is not supported directly from JSON.");
    }

    public override void Write(Utf8JsonWriter writer, IErrorHandlingJSObjectReference value, JsonSerializerOptions options)
    {
        JSObjectReferenceJsonWorker.WriteJSObjectReference(writer, (JSObjectReference)value.JSReference);
    }
}
