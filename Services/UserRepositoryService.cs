using System.Text.Json;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.ServiceInterfaces;

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

    public async Task<bool> AddUserAsync(User user)
    {
        users.Add(user);
        await SaveChangesInUserListAsync();
        return users.Contains(user);
    }

    private async Task SaveChangesInUserListAsync()
    {
        if (!File.Exists(FilePath))
        {
            using (File.Create(FilePath)) { }
        }

        string json = JsonSerializer.Serialize(users, _jsonOptions);
        await File.WriteAllTextAsync(FilePath, json);
    }
}
