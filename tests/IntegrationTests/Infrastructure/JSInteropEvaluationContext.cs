using BlazorServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace IntegrationTests.Infrastructure;

public class JSInteropEvaluationContext(IJSRuntime jSRuntime, IErrorHandlingJSRuntime errorHandlingJSRuntime) : EvaluationContext
{
    public IJSRuntime JSRuntime => jSRuntime;
    public IErrorHandlingJSRuntime ErrorHandlingJSRuntime => errorHandlingJSRuntime;

    public static JSInteropEvaluationContext Create(IServiceProvider provider)
    {
        IJSRuntime jSRuntime = provider.GetRequiredService<IJSRuntime>();
        IErrorHandlingJSRuntime errorHandlingJSRuntime = provider.GetRequiredService<IErrorHandlingJSRuntime>();

        return new JSInteropEvaluationContext(jSRuntime, errorHandlingJSRuntime);
    }
}
