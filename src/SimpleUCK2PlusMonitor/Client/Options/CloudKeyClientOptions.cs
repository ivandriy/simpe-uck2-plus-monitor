namespace SimpleUCK2PlusMonitor.Client.Options;

public class CloudKeyClientOptions
{
    public const string ConfigSectionName = "CloudKey";
    public string Url { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public TimeSpan Timeout { get; set; }
}