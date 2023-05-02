namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// Indicate that an invalid reference has been detected.
/// </summary>
/// <remarks><see href="ReferenceErrorException">See the ECMAScript definition here</see></remarks>
public class ReferenceErrorException : NativeErrorException
{
    /// <inheritdoc/>
    protected ReferenceErrorException(string message, Exception innerException) : base(message, innerException) { }
}
