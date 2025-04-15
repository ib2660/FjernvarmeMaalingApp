using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using System.Text.Json;

namespace FjernvarmeMaalingApp.Services;
public static class JsonHelper
{
    private static readonly JsonSerializerOptions _options = new()
    {
        //WriteIndented = true,
        Converters = { new DateOnlyJsonConverter() }
    };

    public static string SerializeObject<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }

    public static T DeserializeObject<T>(string json)
    {
        var result = JsonSerializer.Deserialize<T>(json, _options);
        if (result == null)
        {
            throw new JsonException("Deserialization failed.");
        }
        return result;
    }

    public static IUser DeserializeUser(string json)
    {
        var result = JsonSerializer.Deserialize<User>(json, _options);
        if (result == null)
        {
            throw new JsonException("Deserialization failed.");
        }
        return result;
    }
}
