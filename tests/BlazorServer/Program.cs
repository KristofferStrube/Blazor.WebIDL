namespace BlazorServer;

public class Program
{
    private static async Task Main(string[] args)
    {
        IHost host = BuildWebHost(args, _ => { });
        await host.RunAsync();
    }

    public static IHost BuildWebHost(string[] args, Action<IServiceCollection> configureServices)
    {
        return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder => builder
                    .UseStaticWebAssets()
                    .UseStartup<Startup>()
                    .ConfigureServices(configureServices)
                )
                .Build();
    }
}