using System.Diagnostics.Metrics;

namespace SimpleUCK2PlusMonitor.Services.Metrics;

public class CloudKeyMetrics
{
    public const string MetricName = "CloudKey";
    public double CpuTemperature { get; set; }
    public double HddTemperature { get; set; }
    public CloudKeyMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MetricName);
        meter.CreateObservableGauge("cloudkey.cpu.temperature", () => CpuTemperature);
        meter.CreateObservableGauge("cloudkey.hdd.temperature", () => HddTemperature);
    }
}