using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleUCK2PlusMonitor.Services.Monitoring;
using SimpleUCK2PlusMonitor.Services.Options;

namespace SimpleUCK2PlusMonitor.Services;

public class Worker : BackgroundService
{
    
    private readonly ILogger<Worker> _logger;
    private readonly WorkerOptions _options;

    public Worker(IServiceProvider serviceProvider, IOptions<WorkerOptions> options, ILogger<Worker> logger)
    {
        Services = serviceProvider;
        _logger = logger;
        _options = options.Value;
    }
    
    public IServiceProvider Services { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker service started");
        
        await UpdateData();
        using PeriodicTimer timer = new(_options.PullingInterval);
        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await UpdateData();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Worker service is stopping.");
        }
    }
    
    private async Task UpdateData()
    {
        using var scope = Services.CreateScope();
        var monitoringService = 
            scope.ServiceProvider
                .GetRequiredService<IMonitoringService>();

        await monitoringService.GetData();
    }
}