using Microsoft.JSInterop;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Class that contains all details of the JS error that happened.
/// </summary>
public class JSError
{
    /// <summary>
    /// Creates a class that holds all information about a JS error.
    /// </summary>
    /// <param name="name">The name of the error.</param>
    /// <param name="message">The message explaining the details of the error.</param>
    /// <param name="stack">The stack trace that leads to which line an error happened on.</param>
    public JSError(string name, string message, string? stack)
    {
        Name = name;
        Message = message;
        Stack = stack;
    }

    /// <summary>
    /// The name of the error.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// The message explaining the details of the error.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }

    /// <summary>
    /// The stack trace that leads to which line an error happened on.
    /// </summary>
    [JsonPropertyName("stack")]
    public string? Stack { get; set; }

    /// <summary>
    /// The extra properties that were requested by setting <see cref="ErrorHandlingJSInterop.ExtraErrorProperties"/>.
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>
    /// The exception that held this information.
    /// </summary>
    public JSException InnerException { get; set; } = default!;
}