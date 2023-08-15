using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Extension methods for the Service Provider.
/// </summary>
public static class IServiceProviderExtensions
{
    /// <summary>
    /// Sets up the needed instances for there to be made Error Handling JS Interop calls in WebAssembly.
    /// </summary>
    /// <param name="serviceProvider">A built service provider.</param>
    /// <returns>The same <paramref name="serviceProvider"/>.</returns>
    public static async Task<IServiceProvider> SetupErrorHandlingJSInterop(this IServiceProvider serviceProvider)
    {
        IJSRuntime? jSRuntime = serviceProvider.GetService<IJSRuntime>();
        if (jSRuntime is IJSInProcessRuntime)
        {
            ErrorHandlingJSInterop.Helper = await jSRuntime.GetInProcessHelperAsync();
        }
        return serviceProvider;
    }
}
