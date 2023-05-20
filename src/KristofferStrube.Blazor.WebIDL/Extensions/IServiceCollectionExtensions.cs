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
        return services.AddScoped<IErrorHandlingJSRuntime>(sp =>
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
            })
            .AddScoped<IErrorHandlingJSInProcessRuntime, ErrorHandlingJSInProcessRuntime>();
    }
}
