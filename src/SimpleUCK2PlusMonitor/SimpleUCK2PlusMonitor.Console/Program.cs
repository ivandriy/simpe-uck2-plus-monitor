using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleUCK2PlusMonitor.Client.Extensions;
using SimpleUCK2PlusMonitor.Services.Monitoring;

var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddEnvironmentVariables();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddCloudKeyClient(builder.Configuration);
builder.Services.AddSingleton<IMonitoringService, CloudKeyMonitoringService>();

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

