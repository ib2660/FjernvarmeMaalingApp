using System.Text.Json;
using System.Text.Json.Serialization;
using FjernvarmeMaalingApp.Services.Converters;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FjernvarmeMaalingApp.Extensions;

public static class JsonSerializerExtensions
{
    public static IServiceCollection AddCustomJsonOptions(this IServiceCollection services)
    {
        services.AddSingleton<JsonSerializerOptions>(provider =>
        {
            var factories = provider.GetServices<IConsumptionTypeFactory>();
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                IncludeFields = true
            };
            jsonOptions.Converters.Add(new ConsumptionTypeJsonConverter(factories));
            return jsonOptions;
        });
        return services;
    }
}
