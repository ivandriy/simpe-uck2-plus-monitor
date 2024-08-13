using System.Diagnostics.Metrics;

namespace SimpleUCK2PlusMonitor.Services.Metrics;

public class CloudKeyMetrics
{
    public const string MetricName = "CloudKey";
    public double CpuTemperature { get; set; }
    public double CpuLoad { get; set; }
    public long DiskSize { get; set; }
    public long DiskUsed { get; set; }
    public long DiskAvailable { get; set; }
    public long HddSize { get; set; }
    public double HddTemperature { get; set; }
    public double HddPowerOnHours { get; set; }
    public int HddBadSectors { get; set; }
    public int HddSmartErrors { get; set; }
    public int HddReadErrors { get; set; }
    public long TotalMemory { get; set; }
    public long FreeMemory { get; set; }
    public long AvailableMemory { get; set; }
    
    public CloudKeyMetrics(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MetricName);
        
        //CPU
        meter.CreateObservableGauge("cloudkey.cpu.temperature", () => CpuTemperature);
        meter.CreateObservableGauge("cloudkey.cpu.load", () => CpuLoad);
        
        //HDD
        meter.CreateObservableGauge("cloudkey.hdd.size", () => HddSize);
        meter.CreateObservableGauge("cloudkey.hdd.temperature", () => HddTemperature);
        meter.CreateObservableGauge("cloudkey.hdd.power_on_hours", () => HddPowerOnHours);
        meter.CreateObservableGauge("cloudkey.hdd.bad_sectors", () => HddBadSectors);
        meter.CreateObservableGauge("cloudkey.hdd.smart_errors", () => HddSmartErrors);
        meter.CreateObservableGauge("cloudkey.hdd.read_errors", () => HddReadErrors);
        
        //Disk
        meter.CreateObservableGauge("cloudkey.disk.size", () => DiskSize);
        meter.CreateObservableGauge("cloudkey.disk.available", () => DiskAvailable);
        meter.CreateObservableGauge("cloudkey.disk.used", () => DiskUsed);
        meter.CreateObservableGauge("cloudkey.disk.health", () => DiskUsed);
        
        //Memory
        meter.CreateObservableGauge("cloudkey.memory.total", () => TotalMemory);
        meter.CreateObservableGauge("cloudkey.memory.free", () => FreeMemory);
        meter.CreateObservableGauge("cloudkey.memory.available", () => AvailableMemory);
    }
}