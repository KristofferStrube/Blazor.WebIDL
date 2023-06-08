using KristofferStrube.Blazor.WebIDL;
using KristofferStrube.Blazor.WebIDL.WasmExample;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddErrorHandlingJSRuntime();

WebAssemblyHost app = builder.Build();

// We need to do this in WASM a single time.
await app.Services.SetupErrorHandlingJSInterop();

await app.RunAsync();
