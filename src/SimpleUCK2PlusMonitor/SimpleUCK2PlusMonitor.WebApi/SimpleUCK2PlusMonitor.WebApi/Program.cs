using OpenTelemetry.Metrics;
using Serilog;
using SimpleUCK2PlusMonitor.Client.Extensions;
using SimpleUCK2PlusMonitor.Services;
using SimpleUCK2PlusMonitor.Services.Metrics;
using SimpleUCK2PlusMonitor.Services.Monitoring;
using SimpleUCK2PlusMonitor.Services.Options;
using static SimpleUCK2PlusMonitor.Services.Metrics.CloudKeyMetrics;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

builder.Logging.ClearProviders();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

builder.Services.AddOpenTelemetry()
    .WithMetrics(providerBuilder =>
    {
        providerBuilder.AddMeter(MetricName);
        providerBuilder.AddPrometheusExporter();
        providerBuilder.AddConsoleExporter();
    });

builder.Services.AddCloudKeyClient(builder.Configuration);
builder.Services.AddSingleton<IMonitoringService, CloudKeyMonitoringService>();
builder.Services.AddSingleton<CloudKeyMetrics>();
builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(WorkerOptions.ConfigSectionName));
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

app.MapPrometheusScrapingEndpoint();

app.MapGet("/api/data", async (IMonitoringService monitoringService) => await monitoringService.GetData());

app.Run();