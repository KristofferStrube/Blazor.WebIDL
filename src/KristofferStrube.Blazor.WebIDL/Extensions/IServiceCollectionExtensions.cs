using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Contains extension methods for adding services to a <see cref="IServiceCollection"/>.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="IErrorHandlingJSRuntime"/> to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns></returns>
    public static IServiceCollection AddErrorHandlingJSRuntime(this IServiceCollection services)
    {
        // Check if we are in WASM and if we are then also register the InProcess variant of the interfaces.
        var serviceProvider = services.BuildServiceProvider();
        if (serviceProvider.GetRequiredService<IJSRuntime>() is IJSInProcessRuntime)
        {
            return services
                .AddScoped<IErrorHandlingJSInProcessRuntime, ErrorHandlingJSInProcessRuntime>()
                .AddScoped(sp => (IErrorHandlingJSRuntime)sp.GetRequiredService<IErrorHandlingJSInProcessRuntime>());
        }
        else
        {
            return services.AddScoped<IErrorHandlingJSRuntime, ErrorHandlingJSRuntime>();
        }
    }
}
