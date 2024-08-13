using System.Text.Json.Serialization;

namespace SimpleUCK2PlusMonitor.Client.Response;

public class SystemInfoResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("hostname")]
    public string Hostname { get; set; }
    
    [JsonPropertyName("ip")]
    public string IpAddress { get; set; }
    
    [JsonPropertyName("mac")]
    public string MacAddress { get; set; }
    
    [JsonPropertyName("ssh")]
    public bool SshEnabled { get; set; }
    
    [JsonPropertyName("publicIp")]
    public string PublicIp { get; set; }
    
    [JsonPropertyName("hasInternet")]
    public bool HasInternet { get; set; }
    
    [JsonPropertyName("backupEnabled")]
    public bool BackupEnabled { get; set; }

    [JsonPropertyName("backupSchedule")]
    public BackupSchedule BackupSchedule { get; set; }
    
    [JsonPropertyName("ustorage")]
    public UStorage UStorage { get; set; }
    
    [JsonPropertyName("hardware")]
    public Hardware Hardware { get; set; }
    
    [JsonPropertyName("memory")]
    public Memory Memory { get; set; }
    
    [JsonPropertyName("ports")]
    public Ports Ports { get; set; }
    
    [JsonPropertyName("storage")]
    public IEnumerable<Storage> Storage { get; set; }
    
    [JsonPropertyName("cpu")]
    public Cpu Cpu { get; set; }

    public override string ToString() => $"Hostname={Hostname}; Temperature={Cpu.Temperature}C; HDD Temperature={UStorage.Disks.FirstOrDefault().Temperature}C";
}

public class BackupSchedule
{
    [JsonPropertyName("frequency")]
    public string Frequency { get; set; }
    
    [JsonPropertyName("day")]
    public int Day { get; set; }

    [JsonPropertyName("hour")]
    public int Hour { get; set; }
}

public class UStorage
{
    [JsonPropertyName("disks")]
    public IEnumerable<Disk> Disks { get; set; }
    
    [JsonPropertyName("space")]
    public IEnumerable<Space> Space { get; set; }
}

public class Disk
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("serial")]
    public string SerialNumber { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }
    
    [JsonPropertyName("rpm")]
    public int Rpm { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("ata")]
    public string Ata { get; set; }
    
    [JsonPropertyName("sata")]
    public string Sata { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
    
    [JsonPropertyName("firmware")]
    public string Firmware { get; set; }

    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
    
    [JsonPropertyName("poweronhrs")]
    public double PowerOnHours { get; set; }
    
    [JsonPropertyName("bad_sector")]
    public int BadSector { get; set; }
    
    [JsonPropertyName("smart_error_count")]
    public int SmartErrorCount { get; set; }
    
    [JsonPropertyName("read_error")]
    public int ReadError { get; set; }

    [JsonPropertyName("healthy")]
    public string Healthy { get; set; }
}

public class Space
{
    [JsonPropertyName("action")]
    public string Action { get; set; }

    [JsonPropertyName("device")]
    public string Device { get; set; }
    
    [JsonPropertyName("errors_count")]
    public int ErrorsCount { get; set; }
    
    [JsonPropertyName("estimate")]
    public object Estimate { get; set; }
    
    [JsonPropertyName("health")]
    public string Health { get; set; }
    
    [JsonPropertyName("progress")]
    public int Progress { get; set; }
    
    [JsonPropertyName("raid")]
    public object Raid { get; set; }
    
    [JsonPropertyName("reasons")]
    public object[] Reasons { get; set; }
    
    [JsonPropertyName("resv_bytes")]
    public long ReservedBytes { get; set; }
    
    [JsonPropertyName("space_type")]
    public string SpaceType { get; set; }
    
    [JsonPropertyName("total_bytes")]
    public long TotalBytes { get; set; }
    
    [JsonPropertyName("used_bytes")]
    public long UsedBytes { get; set; }
}

public class Hardware
{
    [JsonPropertyName("debianCodename")]
    public string DebianCodename { get; set; }
    
    [JsonPropertyName("sysid")]
    public int Sysid { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("shortname")]
    public string Shortname { get; set; }
    
    [JsonPropertyName("subtype")]
    public string Subtype { get; set; }
    
    [JsonPropertyName("reboot")]
    public string Reboot { get; set; }
    
    [JsonPropertyName("upgrade")]
    public string Upgrade { get; set; }
    
    [JsonPropertyName("cpu_id")]
    public string CpuId { get; set; }
    
    [JsonPropertyName("uuid")]
    public string Uuid { get; set; }
    
    [JsonPropertyName("bom")]
    public string Bom { get; set; }
    
    [JsonPropertyName("hwrev")]
    public int HardwareRev { get; set; }
    
    [JsonPropertyName("mac")]
    public string MacAddress { get; set; }
    
    [JsonPropertyName("serialno")]
    public string SerialNumber { get; set; }
    
    [JsonPropertyName("qrid")]
    public string Qrid { get; set; }
    
    [JsonPropertyName("firmwareVersion")]
    public string FirmwareVersion { get; set; }
}

public class Memory
{
    [JsonPropertyName("free")]
    public long Free { get; set; }
    
    [JsonPropertyName("total")]
    public long Total { get; set; }
    
    [JsonPropertyName("available")]
    public long Available { get; set; }
}

public class Ports
{
    [JsonPropertyName("https")]
    public int Https { get; set; }
    
    [JsonPropertyName("grpc")]
    public int Grpc { get; set; }
    
    [JsonPropertyName("ipc")]
    public int Ipc { get; set; }
    
    [JsonPropertyName("stackable")]
    public int Stackable { get; set; }
    
    [JsonPropertyName("consoleGroup")]
    public int ConsoleGroup { get; set; }
}

public class Storage
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("mounted")]
    public bool Mounted { get; set; }
    
    [JsonPropertyName("mountPoint")]
    public string MountPoint { get; set; }
    
    [JsonPropertyName("size")]
    public long? Size { get; set; }
    
    [JsonPropertyName("used")]
    public long? Used { get; set; }
    
    [JsonPropertyName("avail")]
    public long? Available { get; set; }
    
    [JsonPropertyName("device")]
    public StorageDevice Device { get; set; }
}

public class StorageDevice
{
    [JsonPropertyName("mounted")]
    public bool IsMounted { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("serial")]
    public string SerialNumber { get; set; }
    
    [JsonPropertyName("firmware")]
    public string Firmware { get; set; }
    
    [JsonPropertyName("size")]
    public ulong? Size { get; set; }
    
    [JsonPropertyName("rpm")]
    public int Rpm { get; set; }
    
    [JsonPropertyName("healthy")]
    public bool IsHealthy { get; set; }
    
    [JsonPropertyName("ataVersion")]
    public string AtaVersion { get; set; }
    
    [JsonPropertyName("sataVersion")]
    public string SataVersion { get; set; }
}

public class Cpu
{
    [JsonPropertyName("model")]
    public string Model { get; set; }
    
    [JsonPropertyName("currentload")]
    public double CurrentLoad { get; set; }
    
    [JsonPropertyName("temperature")]
    public double Temperature { get; set; }
}










