using OpenTelemetry.Metrics;
using SimpleUCK2PlusMonitor.Client.Extensions;
using SimpleUCK2PlusMonitor.Services;
using static SimpleUCK2PlusMonitor.Services.CloudKeyMetrics;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddEnvironmentVariables();
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddOpenTelemetry()
    .WithMetrics(providerBuilder =>
    {
        providerBuilder.AddMeter(MetricName);
        providerBuilder.AddPrometheusExporter();
        providerBuilder.AddConsoleExporter();
    });

builder.Services.AddCloudKeyClient(builder.Configuration);
builder.Services.AddSingleton<IMonitoringService, MonitoringService>();
builder.Services.AddSingleton<CloudKeyMetrics>();

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

app.MapGet("/api/data", async (IMonitoringService monitoringService) => await monitoringService.GetData());

app.Run();