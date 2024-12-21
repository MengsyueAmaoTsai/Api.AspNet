using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Infrastructure.Brokerages.Binance;

public static class BinanceBrokerageServiceExtensions
{
    public static IServiceCollection AddBinanceBrokerage(this IServiceCollection services)
    {
        return services;
    }
}