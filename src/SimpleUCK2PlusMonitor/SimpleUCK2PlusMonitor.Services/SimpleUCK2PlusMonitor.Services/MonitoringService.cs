using System.Net;
using Microsoft.Extensions.Logging;
using SimpleUCK2PlusMonitor.Client;
using SimpleUCK2PlusMonitor.Client.Response;

namespace SimpleUCK2PlusMonitor.Services;

public class MonitoringService : IMonitoringService
{
    private readonly ICloudKeyClient _client;
    private readonly CloudKeyMetrics _metrics;
    private readonly ILogger<MonitoringService> _logger;

    public MonitoringService(ICloudKeyClient client, CloudKeyMetrics metrics, ILogger<MonitoringService> logger)
    {
        _client = client;
        _metrics = metrics;
        _logger = logger;
    }
    
    public async Task<SystemInfoResponse> GetData()
    {
        if (_client.IsLoggedOn)
        {
            return await _client.GetSystemInfo();
        }

        await ForceLogin();
        var result = await _client.GetSystemInfo();
        UpdateMetrics(result);
        return result;
    }


    private async Task Login()
    {
        try
        {
            await _client.Login();
        }
        catch (Exception e)
        {
            _logger.LogCritical("Unable to login to Unify Cloud Key 2 Plus: {Error}", e.Message);
            throw;
        }
    }

    private async Task SelfCheck()
    {
        try
        {
            await _client.Self();
        }
        catch (HttpRequestException e) when(e.StatusCode == HttpStatusCode.Unauthorized)
        {
            _logger.LogCritical("Unable to access Unify Cloud Key 2 Plus API: {Error}", e.Message);
            throw;
        }
    }

    private async Task ForceLogin()
    {
        await Login();
        await SelfCheck();
    }

    private void UpdateMetrics(SystemInfoResponse data)
    {
        _metrics.CpuTemperature = data.Cpu.Temperature;
        _metrics.HddTemperature = data.UStorage.Disks.FirstOrDefault().Temperature;
    }
}