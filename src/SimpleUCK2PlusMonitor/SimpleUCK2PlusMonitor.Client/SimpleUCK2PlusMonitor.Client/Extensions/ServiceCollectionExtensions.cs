using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleUCK2PlusMonitor.Client.Options;

namespace SimpleUCK2PlusMonitor.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCloudKeyClient(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetRequiredSection(CloudKeyClientOptions.ConfigSectionName).Get<CloudKeyClientOptions>();
        services.AddHttpClient<ICloudKeyClient, CloudKey2PlusClient>(client =>
        {
            if (config?.Url != null) client.BaseAddress = new Uri(config.Url);
            if (config != null) client.Timeout = config.Timeout;
        }).ConfigurePrimaryHttpMessageHandler(() =>
        {
            return new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, certChain, policyErrors) => true
            };
        });
        services.Configure<CloudKeyClientOptions>(configuration.GetSection(CloudKeyClientOptions.ConfigSectionName));
        return services;
    }
}