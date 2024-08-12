using SimpleUCK2PlusMonitor.Client.Response;

namespace SimpleUCK2PlusMonitor.Services.Monitoring;

public interface IMonitoringService
{
    Task<SystemInfoResponse> GetData();
}