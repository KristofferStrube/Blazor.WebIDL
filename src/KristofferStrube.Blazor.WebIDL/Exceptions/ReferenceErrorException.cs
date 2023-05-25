namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// Indicate that an invalid reference has been detected.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-native-error-types-used-in-this-standard-referenceerror">See the ECMAScript definition here</see></remarks>
public class ReferenceErrorException : WebIDLException
{
    /// <inheritdoc/>
    public ReferenceErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, jSStackTrace, innerException) { }
}
