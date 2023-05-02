namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// Indicates that one of the global URI handling functions was used in a way that is incompatible with its definition.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-native-error-types-used-in-this-standard-urierror">See the ECMAScript definition here</see></remarks>
public class URIErrorException : NativeErrorException
{
    /// <inheritdoc/>
    protected URIErrorException(string message, Exception innerException) : base(message, innerException) { }
}
