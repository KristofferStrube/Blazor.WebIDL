namespace KristofferStrube.Blazor.WebIDL.Exceptions;

/// <summary>
/// A common exception class for all exceptions from the <c>Blazor.WebIDL</c> library.
/// </summary>
public class WebIDLException : Exception
{
    /// <inheritdoc/>
    public WebIDLException(string message, Exception innerException) : base(message, innerException) { }
}
