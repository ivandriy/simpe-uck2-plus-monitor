using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleUCK2PlusMonitor.Services.Monitoring;
using SimpleUCK2PlusMonitor.Services.Options;

namespace SimpleUCK2PlusMonitor.Services;

public class Worker : BackgroundService
{
    private readonly IMonitoringService _monitoringService;
    private readonly ILogger<Worker> _logger;
    private readonly WorkerOptions _options;

    public Worker(IMonitoringService monitoringService, IOptions<WorkerOptions> options, ILogger<Worker> logger)
    {
        _monitoringService = monitoringService;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker service started");
        await RunMonitoring();
       
        using PeriodicTimer timer = new(_options.PullingInterval);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await RunMonitoring();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Worker service is stopping.");
        }
    }

    private async Task RunMonitoring()
    {
        _logger.LogInformation("Update monitoring data");
        await _monitoringService.GetData();
    }
}