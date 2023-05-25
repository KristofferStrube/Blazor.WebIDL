namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// When an ECMAScript implementation detects a runtime error, it throws a new instance of one of the NativeError objects. All deriving Errors have the same structure as this error, differing only in the name used as the constructor name instead of NativeError, in the "name" property of the prototype object, and in the implementation-defined "message" property of the prototype object.
/// </summary>
/// <remarks><see href="https://tc39.es/ecma262/multipage/fundamental-objects.html#sec-nativeerror-object-structure">See the ECMAScript definition here</see></remarks>
public class NativeErrorException : WebIDLException
{
    /// <inheritdoc/>
    protected NativeErrorException(string message, string? jSStackTrace, Exception innerException) : base(message, jSStackTrace, innerException) { }
}
