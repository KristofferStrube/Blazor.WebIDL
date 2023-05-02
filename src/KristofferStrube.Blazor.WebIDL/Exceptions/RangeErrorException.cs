namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// Indicates a value that is not in the set or range of allowable values.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-native-error-types-used-in-this-standard-rangeerror">See the ECMAScript definition here</see></remarks>
public class RangeErrorException : NativeErrorException
{
    /// <inheritdoc/>
    protected RangeErrorException(string message, Exception innerException) : base(message, innerException) { }
}
