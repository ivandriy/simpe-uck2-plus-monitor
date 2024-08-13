using System.Net;
using Microsoft.Extensions.Logging;
using SimpleUCK2PlusMonitor.Client;
using SimpleUCK2PlusMonitor.Client.Response;
using SimpleUCK2PlusMonitor.Services.Metrics;

namespace SimpleUCK2PlusMonitor.Services.Monitoring;

public class CloudKeyMonitoringService : IMonitoringService
{
    private readonly ICloudKeyClient _client;
    private readonly CloudKeyMetrics _metrics;
    private readonly ILogger<CloudKeyMonitoringService> _logger;

    public CloudKeyMonitoringService(ICloudKeyClient client, CloudKeyMetrics metrics, ILogger<CloudKeyMonitoringService> logger)
    {
        _client = client;
        _metrics = metrics;
        _logger = logger;
    }
    
    public async Task<SystemInfoResponse> GetData()
    {
        if (_client.IsLoggedOn)
        {
            return await GetDataInternal();
        }

        await ForceLogin();
        return await GetDataInternal();
    }


    private async Task Login()
    {
        _logger.LogDebug("Try login");
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
        _logger.LogDebug("Try self check");
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

    private async Task<SystemInfoResponse> GetDataInternal()
    {
        _logger.LogDebug("Get data");
        var result = await _client.GetSystemInfo();
        UpdateMetrics(result);
        return result;
    }

    private void UpdateMetrics(SystemInfoResponse data)
    {
        var cpuTemperature = data.Cpu.Temperature;
        var cpuLoad = data.Cpu.CurrentLoad;
        var hdd = data.UStorage.Disks.FirstOrDefault();
        var disk = data.Storage.FirstOrDefault(s => s.Type == "hdd");
        var hddTemperature = hdd?.Temperature;
        var hddSize = hdd?.Size;
        var hddPwrOnHrs = hdd?.PowerOnHours;
        var badSectors = hdd?.BadSector;
        var smartErrors = hdd?.SmartErrorCount;
        var readErrors = hdd?.ReadError;
        var diskSize = disk?.Size;
        var diskAvailable = disk?.Available;
        var diskUsed = disk?.Used;
        var totalMemory = data.Memory.Total;
        var freeMemory = data.Memory.Free;
        var availableMemory = data.Memory.Available;
        
        _logger.LogDebug("Current CpuTemperature: {CurrCpu}; Updated CpuTemperature: {UpdCpu}", _metrics.CpuTemperature, cpuTemperature);
        _logger.LogDebug("Current HddTemperature: {CurrHddTemp}; Updated HddTemperature: {UpdHddTemp}", _metrics.CpuTemperature, hddTemperature);
        
        _metrics.CpuTemperature = cpuTemperature;
        _metrics.CpuLoad = cpuLoad;
        _metrics.HddSize = hddSize ?? 0;
        _metrics.HddTemperature = hddTemperature ?? 0;
        _metrics.HddPowerOnHours = hddPwrOnHrs ?? 0;
        _metrics.HddBadSectors = badSectors ?? 0;
        _metrics.HddSmartErrors = smartErrors ?? 0;
        _metrics.HddReadErrors = readErrors ?? 0;
        _metrics.DiskSize = diskSize ?? 0;
        _metrics.DiskAvailable = diskAvailable ?? 0;
        _metrics.DiskUsed = diskUsed ?? 0;
        _metrics.TotalMemory = totalMemory;
        _metrics.FreeMemory = freeMemory;
        _metrics.AvailableMemory = availableMemory;
    }
}