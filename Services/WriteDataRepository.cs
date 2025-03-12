using FjernvarmeMaalingApp.Services.ServiceInterfaces;
using FjernvarmeMaalingApp.ViewModels;
using System.Text.Json;

namespace FjernvarmeMaalingApp.Services;

public class WriteDataRepository(ILogger<WriteDataRepository> logger, LoginViewModel loginViewModel) : IWriteDataRepository
{
    private readonly ILogger<WriteDataRepository> Logger = logger;
    private readonly LoginViewModel LoginViewModel = loginViewModel;

    public bool EnterData(string json)
    {
        // Add user information to the JSON string
        var user = LoginViewModel.CurrentUser?.Identity?.Name ?? "Unknown User";
        var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
        jsonData["RegisteredBy"] = user;
        var updatedJson = JsonSerializer.Serialize(jsonData);

        try
        {
            // Implement method to add data to a file - for demonstration purposes
            File.AppendAllText("data.json", updatedJson + Environment.NewLine);
            return true;
        }
        catch (Exception ex)
        {
            // Implement logging
            Logger.LogError(ex, "An error occurred while entering data.");
            return false;
        }
    }

    public bool UpdateData(string json)
    {
        throw new NotImplementedException();
    }
}
