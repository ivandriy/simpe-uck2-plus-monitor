namespace SimpleUCK2PlusMonitor.Services.Options;

public class WorkerOptions
{
    public const string ConfigSectionName = "Worker";
    
    public TimeSpan PullingInterval { get; set; }
}