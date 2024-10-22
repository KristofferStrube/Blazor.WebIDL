using BlazorServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace IntegrationTests.Infrastructure;

public class JSInteropEvaluationContext(IJSRuntime jSRuntime) : EvaluationContext, IEvaluationContext<JSInteropEvaluationContext>
{
    public IJSRuntime JSRuntime => jSRuntime;

    public static JSInteropEvaluationContext Create(IServiceProvider provider)
    {
        IJSRuntime jSRuntime = provider.GetRequiredService<IJSRuntime>();

        return new JSInteropEvaluationContext(jSRuntime);
    }
}
