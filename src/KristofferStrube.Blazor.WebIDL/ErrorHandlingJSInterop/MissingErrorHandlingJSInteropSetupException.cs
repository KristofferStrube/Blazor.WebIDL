namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// An exception that is thrown if the needed instances were not setup for there to be made Error Handling JS Interop using the <c>Blazor.WebIDL</c> library
/// </summary>
public class MissingErrorHandlingJSInteropSetupException : Exception
{
    /// <summary>
    /// creates a new instance of the exception
    /// </summary>
    internal MissingErrorHandlingJSInteropSetupException() : base("The needed instances was not setup for Blazor.WebIDL to be able to make Error Handling JS Interop. Please call 'app.Services.SetupErrorHandlingJSInterop();' once after you have build your application.") { }
}
