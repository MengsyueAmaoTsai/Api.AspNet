using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Infrastructure.Brokerages.Max;

public static class MaxBrokerageServiceExtensions
{
    private const string BaseAddress = "https://max-api.maicoin.com";
    private const string MaxApiKey = "aq3hYs749TbrH9620dygXwoxby4TlEYOoDdoBjXH";

    public static IServiceCollection AddMaxBrokerage(this IServiceCollection services)
    {
        services.AddHttpClient<MaxRestService>(client =>
        {
            client.BaseAddress = new Uri(BaseAddress);

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-MAX-ACCESSKEY", MaxApiKey);
        });

        services.AddSingleton<MaxBrokerage>();

        return services;
    }
}