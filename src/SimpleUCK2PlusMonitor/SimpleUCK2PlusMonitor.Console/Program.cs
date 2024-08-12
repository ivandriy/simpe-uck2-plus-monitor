using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleUCK2PlusMonitor.Client;
using SimpleUCK2PlusMonitor.Client.Options;
using SimpleUCK2PlusMonitor.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddEnvironmentVariables();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

var config = builder.Configuration.GetRequiredSection(CloudKeyClientOptions.ConfigSectionName).Get<CloudKeyClientOptions>();
builder.Services.AddHttpClient<ICloudKeyClient, CloudKey2PlusClient>(client =>
{
    if (config?.Url != null) client.BaseAddress = new Uri(config.Url);
    if (config != null) client.Timeout = config.Timeout;
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ClientCertificateOptions = ClientCertificateOption.Manual,
        ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, certChain, policyErrors) => true
    };
});
builder.Services.Configure<CloudKeyClientOptions>(builder.Configuration.GetSection(CloudKeyClientOptions.ConfigSectionName));
builder.Services.AddSingleton<IMonitoringService, MonitoringService>();

using var host = builder.Build();
await GetMonitoringData(host.Services);
await host.RunAsync();


static async Task GetMonitoringData(IServiceProvider hostProvider)
{
    using var serviceScope = hostProvider.CreateScope();
    var provider = serviceScope.ServiceProvider;
    var monitoringService = provider.GetRequiredService<IMonitoringService>();
    var data = await monitoringService.GetData();
    
    Console.WriteLine("Unify Cloud Key data received:");
    Console.WriteLine(data.ToString());
}

