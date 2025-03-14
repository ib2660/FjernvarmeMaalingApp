using System.Text.Json;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Services;

public class UserRepositoryService : IUserRepository
{
    private readonly Dictionary<string, IConsumptionTypeFactory> ConsumptionTypeFactories;
    private readonly ILogger<UserRepositoryService> Logger;
    private const string FilePath = "users.json";
    private readonly JsonSerializerOptions _jsonOptions;
    private List<User> users;

    public UserRepositoryService(ILogger<UserRepositoryService> logger, JsonSerializerOptions jsonOptions, IEnumerable<IConsumptionTypeFactory> factories)
    {
        Logger = logger;
        _jsonOptions = jsonOptions;
        ConsumptionTypeFactories = factories.ToDictionary(factory => factory.ConsumptionTypeName, factory => factory);
        try
        {
            string json = File.ReadAllText(FilePath);
            users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions) ?? [];
            foreach (var user in users)
            {
                user.PreferredConsumptionType = GetConsumptionTypeByName(user.PreferredConsumptionTypeName);
            }
            Logger.LogInformation("Users loaded from JSON file.");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to deserialize users from JSON file.");
            users = [];
        }
    }

    private IConsumptionType? GetConsumptionTypeByName(string name)
    {
        if (ConsumptionTypeFactories.TryGetValue(name, out var factory))
        {
            return factory.CreateConsumptionType();
        }
        else return null;
    }

    public async Task<User?> GetUserAsync(string username)
    {
        User? foundUser = users.FirstOrDefault(u => u.Username == username);
        return foundUser;
    }

    private async Task SaveChangesInUserListAsync()
    {
        if (!File.Exists(FilePath))
        {
            using (File.Create(FilePath))
            {
                Logger.LogInformation("User list file created.");
            }
        }
        string json = JsonSerializer.Serialize(users, _jsonOptions);
        await File.WriteAllTextAsync(FilePath, json);
        Logger.LogInformation("Changes saved to user list.");
    }

    public async Task<bool> AddOrUpdateUserAsync(User user)
    {
        var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            users.Remove(existingUser);
            users.Add(user);
            await SaveChangesInUserListAsync();
            Logger.LogInformation($"User {user.Username} updated.");
            return true;
        }
        else
        {
            users.Add(user);
            await SaveChangesInUserListAsync();
            if (users.Contains(user))
            {
                Logger.LogInformation($"User {user.Username} added to list.");
                return true;
            }
            else
            {
                Logger.LogError("Failed to add user to list.");
                return false;
            }
        }
    }
}
