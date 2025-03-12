using System.Text.Json;
using System.Text.Json.Serialization;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Converters;

public class ConsumptionTypeJsonConverter : JsonConverter<IConsumptionType>
{
    private readonly Dictionary<string, IConsumptionTypeFactory> _factories;

    public ConsumptionTypeJsonConverter(IEnumerable<IConsumptionTypeFactory> factories)
    {
        _factories = factories.ToDictionary(factory => factory.ConsumptionTypeName, factory => factory);
    }

    public override IConsumptionType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonObject = JsonDocument.ParseValue(ref reader).RootElement;
        var consumptionTypeName = jsonObject.GetProperty("preferredConsumptionTypeName").GetString();
        if (consumptionTypeName == "")
        { 
            return null; 
        }
        if (consumptionTypeName != "" && _factories.TryGetValue(consumptionTypeName, out var factory))
        {
            var consumptionType = factory.CreateConsumptionType();
            return consumptionType;
        }

        throw new JsonException($"Unknown consumption type: {consumptionTypeName}");
    }

    public override void Write(Utf8JsonWriter writer, IConsumptionType value, JsonSerializerOptions options)
    {

        writer.WriteStartObject();
        writer.WriteString("preferredConsumptionTypeName", value.ConsumptionTypeName ?? "");
        writer.WriteEndObject();
    }
}