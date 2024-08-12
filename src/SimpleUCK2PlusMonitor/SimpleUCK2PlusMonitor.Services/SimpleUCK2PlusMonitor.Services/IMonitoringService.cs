using SimpleUCK2PlusMonitor.Client.Response;

namespace SimpleUCK2PlusMonitor.Services;

public interface IMonitoringService
{
    Task<SystemInfoResponse> GetData();
}