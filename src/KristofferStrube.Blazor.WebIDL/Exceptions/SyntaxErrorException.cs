namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// Indicates that a parsing error has occurred.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-native-error-types-used-in-this-standard-syntaxerror">See the ECMAScript definition here</see></remarks>
public class SyntaxErrorException : NativeErrorException
{
    /// <inheritdoc/>
    public SyntaxErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, jSStackTrace, innerException) { }
}
