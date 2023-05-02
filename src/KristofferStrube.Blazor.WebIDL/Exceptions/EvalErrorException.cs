namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// This exception is not currently used within this specification. This object remains for compatibility with previous editions of this specification.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-native-error-types-used-in-this-standard-evalerror">See the ECMAScript definition here</see></remarks>
public class EvalErrorException : NativeErrorException
{
    /// <inheritdoc/>
    protected EvalErrorException(string message, Exception innerException) : base(message, innerException) { }
}
