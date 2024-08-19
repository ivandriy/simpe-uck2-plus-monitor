using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using Serilog;
using SimpleUCK2PlusMonitor.Client.HealthChecks;
using SimpleUCK2PlusMonitor.Client.Options;

namespace SimpleUCK2PlusMonitor.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCloudKeyClient(this IServiceCollection services, IConfiguration configuration)
    {
        var logger = Log.ForContext<CloudKey2PlusClient>();
        var config = configuration.GetRequiredSection(CloudKeyClientOptions.ConfigSectionName).Get<CloudKeyClientOptions>();
        services.AddHttpClient<ICloudKeyClient, CloudKey2PlusClient>(client =>
        {
            if (config?.Url != null) client.BaseAddress = new Uri(config.Url);
        })
            .SetHandlerLifetime(config?.HandlerLifeTime ?? TimeSpan.FromMinutes(5))
            .AddPolicyHandler(GetRetryPolicy(logger, config?.RetryCount ?? 10))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(config?.Timeout.Seconds ?? 5))
            .ConfigurePrimaryHttpMessageHandler(() =>
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

    public static IServiceCollection AddCloudKeyHealthCheck(this IServiceCollection services)
    {
        services
            .AddSingleton<CloudKeyHealthCheck>();
        services
            .AddHealthChecks()
            .AddCheck<CloudKeyHealthCheck>(nameof(CloudKeyHealthCheck), tags: new[] { "cloudkey" });

        return services;
    }
    
    
    
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger, int retryCount)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (result, timespan, retryAttempt, _) =>
                {
                    logger.Warning("Retry attempt {Attempt} after {TotalSeconds} seconds due to: {ExMessage}",
                        retryAttempt, timespan.TotalSeconds, result.Exception.Message);
                });
    }
}