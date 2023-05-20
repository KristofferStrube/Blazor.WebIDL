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
        services.AddScoped<IErrorHandlingJSRuntime>(sp =>
            {
                IJSRuntime jSRuntime = sp.GetRequiredService<IJSRuntime>();
                if (jSRuntime is JSInProcessRuntime)
                {
                    return new ErrorHandlingJSInProcessRuntime();
                }
                else
                {
                    return new ErrorHandlingJSRuntime();
                }
            });

        // Check if we are in WASM and if we are then also register the InProcess variant of the interfaces.
        var serviceProvider = services.BuildServiceProvider();
        if (serviceProvider.GetRequiredService<IJSRuntime>() is IJSInProcessRuntime)
        {
            services.AddScoped<IErrorHandlingJSInProcessRuntime, ErrorHandlingJSInProcessRuntime>();
        }

        return services;
    }
}
