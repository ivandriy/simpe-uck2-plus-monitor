using SimpleUCK2PlusMonitor.Client.Response;

namespace SimpleUCK2PlusMonitor.Client;

public interface ICloudKeyClient
{
    bool IsLoggedOn { get; }
    
    Task Login();

    Task Self();

    Task<SystemInfoResponse> GetSystemInfo();
}