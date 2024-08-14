using OpenTelemetry.Metrics;
using Serilog;
using SimpleUCK2PlusMonitor.Client.Extensions;
using SimpleUCK2PlusMonitor.Services;
using SimpleUCK2PlusMonitor.Services.Metrics;
using SimpleUCK2PlusMonitor.Services.Monitoring;
using SimpleUCK2PlusMonitor.Services.Options;
using static SimpleUCK2PlusMonitor.Services.Metrics.CloudKeyMetrics;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up the service");
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.Sources.Clear();
    builder.Configuration
        .AddJsonFile("appsettings.json")
        .AddEnvironmentVariables();

    builder.Logging.ClearProviders();
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    
    builder.Host.UseSerilog(logger);
    builder.Logging.AddSerilog(logger);
    builder.Services.AddOpenTelemetry()
        .WithMetrics(providerBuilder =>
        {
            providerBuilder.AddMeter(MetricName);
            providerBuilder.AddPrometheusExporter();
#if DEBUG
            providerBuilder.AddConsoleExporter();
#endif
        });
    builder.Services.AddHealthChecks();

    builder.Services.AddCloudKeyClient(builder.Configuration);
    builder.Services.AddCloudKeyHealthCheck();
    builder.Services.AddSingleton<IMonitoringService, CloudKeyMonitoringService>();
    builder.Services.AddSingleton<CloudKeyMetrics>();
    builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection(WorkerOptions.ConfigSectionName));
    builder.Services.AddHostedService<Worker>();

    var app = builder.Build();
    app.UseSerilogRequestLogging();
    
    app.MapHealthChecks("/health");
    app.MapPrometheusScrapingEndpoint();
    
    app.MapGet("/api/data", async (IMonitoringService monitoringService) => await monitoringService.GetData());
    
    app.Run();
    
    Log.Information("Stopped cleanly");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during startup");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

