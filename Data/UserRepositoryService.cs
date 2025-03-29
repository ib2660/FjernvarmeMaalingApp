using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services;

namespace FjernvarmeMaalingApp.Data;

public class UserRepositoryService : IUserRepository
{
    private readonly ILogger<UserRepositoryService> _logger;
    private const string _filePath = "users.json";
    private readonly List<User> users;

    public UserRepositoryService(ILogger<UserRepositoryService> logger)
    {
        _logger = logger;
        try
        {
            string json = File.ReadAllText(_filePath);
            users = JsonHelper.DeserializeObject<List<User>>(json) ?? [];
            _logger.LogInformation("Users loaded from JSON file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize users from JSON file.");
            users = [];
        }
    }

    public async Task<User?> GetUserAsync(string username)
    {
        User? foundUser = users.FirstOrDefault(u => u.Username == username);
        return foundUser;
    }
    private async Task SaveChangesInUserListAsync()
    {
        if (!File.Exists(_filePath))
        {
            try
            {
                File.Create(_filePath);
                _logger.LogInformation("User list file created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user list file.");
            }
        }
        string json = JsonHelper.SerializeObject(users);
        try
        {
            await File.WriteAllTextAsync(_filePath, json);
            _logger.LogInformation("Changes saved to user list.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write changes to user list.");
        }
    }

    public async Task<bool> AddOrUpdateUserAsync(User user)
    {
        var existingUser = users.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser != null)
        {
            users.Remove(existingUser);
            users.Add(user);
            await SaveChangesInUserListAsync();
            return true;
        }
        else
        {
            user.Id = users.Count + 1;
            users.Add(user);
            await SaveChangesInUserListAsync();
            if (users.Contains(user))
            {
                _logger.LogInformation($"User {user.Username} added to list.");
                return true;
            }
            else
            {
                _logger.LogError("Failed to add user to list.");
                return false;
            }
        }
    }
}
