using Microsoft.Extensions.DependencyInjection;

namespace KristofferStrube.Blazor.WebIDL;

/// <summary>
/// Contains extension methods for adding services to a <see cref="IServiceCollection"/>.
/// </summary>
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds an <see cref="IErrorHandlingJSRuntime"/> to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddErrorHandlingJSRuntime(this IServiceCollection services)
    {
        return services.AddScoped<IErrorHandlingJSRuntime, ErrorHandlingJSRuntime>();
    }
}
