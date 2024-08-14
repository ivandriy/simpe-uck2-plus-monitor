using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SimpleUCK2PlusMonitor.Client.HealthChecks;

public class CloudKeyHealthCheck : IHealthCheck
{
    private readonly ICloudKeyClient _cloudKeyClient;

    public CloudKeyHealthCheck(ICloudKeyClient cloudKeyClient) => _cloudKeyClient = cloudKeyClient;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
    {
        try
        {
            await _cloudKeyClient.GetSystemInfo();
        }
        catch (Exception)
        {
            return HealthCheckResult.Unhealthy("CloudKey endpoint is unhealthy");
        }

        return HealthCheckResult.Healthy("CloudKey endpoint is healthy");
    }
}