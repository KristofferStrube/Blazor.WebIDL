namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// TypeError is used to indicate an unsuccessful operation when none of the other <see cref="NativeErrorException"/> objects are an appropriate indication of the failure cause.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-native-error-types-used-in-this-standard-typeerror">See the ECMAScript definition here</see></remarks>
public class TypeErrorException : NativeErrorException
{
    /// <inheritdoc/>
    public TypeErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, jSStackTrace, innerException) { }
}
