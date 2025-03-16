using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using System.Text.Json;

namespace FjernvarmeMaalingApp.Services;

public class WriteDataRepository(ILogger<WriteDataRepository> logger) : IWriteDataRepository
{
    private readonly ILogger<WriteDataRepository> _logger = logger;
    public async Task<bool> EnterData(string json)
    {
        try
        {
            await File.AppendAllTextAsync("data.json", json + Environment.NewLine); // demonstration af gemning af data i et repository som kan skiftes ud med en database, en cloud storage service eller andet.
            return true;
        }
        catch (Exception ex)
        {
            // Implement logging
            _logger.LogError(ex, "An error occurred while entering data.");
            return false;
        }
    }

    public async Task<bool> UpdateData(string json)
    {
        try
        {
            var data = await File.ReadAllLinesAsync("data.json");
            var updatedData = data.Select(line =>
            {
                var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(line);
                var newData = JsonSerializer.Deserialize<Dictionary<string, object>>(json);
                if (jsonData["id"].ToString() == newData["id"].ToString()) // TODO: data har ikke et felt der hedder id, så det skal ændres. En kombination af RegisteredBy og MeasurementDate kunne benyttes.
                {
                    return json;
                }
                return line;
            }).ToArray();

            await File.WriteAllLinesAsync("data.json", updatedData);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating data.");
            return false;
        }
    }
}
