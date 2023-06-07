using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Text.Json;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Extension methods for the Service Provider.
/// </summary>
public static class IServiceProviderExtensions
{
    /// <summary>
    /// Sets up the needed instances for there to be made Error Handling JS Interop calls.
    /// </summary>
    /// <param name="serviceProvider">A build service provider.</param>
    /// <returns>The same <paramref name="serviceProvider"/>.</returns>
    public static async Task<IServiceProvider> SetupErrorHandlingJSInterop(this IServiceProvider serviceProvider)
    {
        IJSRuntime jSRuntime = serviceProvider.GetRequiredService<IJSRuntime>();
        ErrorHandlingJSInterop.JsonSerializerOptions = new JsonSerializerOptions
        {
            Converters = { new JSObjectReferenceJsonConverter((JSRuntime)jSRuntime) }
        };
        if (jSRuntime is IJSInProcessRuntime)
        {
            ErrorHandlingJSInterop.Helper = await jSRuntime.GetInProcessHelperAsync();
        }
        else
        {
            ErrorHandlingJSInterop.Helper = await jSRuntime.GetHelperAsync();
        }
        return serviceProvider;
    }
}
