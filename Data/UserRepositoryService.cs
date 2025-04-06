using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services;

namespace FjernvarmeMaalingApp.Data;

public class UserRepositoryService : IUserRepository
{
    private readonly ILogger<UserRepositoryService> _logger;
    private const string _filePath = "users.json";
    private readonly List<IUser> users;

    public UserRepositoryService(ILogger<UserRepositoryService> logger)
    {
        _logger = logger;
        try
        {
            string json = File.ReadAllText(_filePath);
            users = JsonHelper.DeserializeObject<List<IUser>>(json) ?? [];
            _logger.LogInformation("Users loaded from JSON file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize users from JSON file.");
            users = [];
        }
    }

    public async Task<IUser?> GetUserAsync(string username)
    {
        IUser? foundUser = users.FirstOrDefault(u => u.Username == username);
        return foundUser;
    }
    private async Task<bool> SaveChangesInUserListAsync()
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
                return false;
            }
        }
        string json = JsonHelper.SerializeObject(users);
        try
        {
            await File.WriteAllTextAsync(_filePath, json);
            _logger.LogInformation("Changes saved to user list.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to write changes to user list.");
            return false;
        }
    }

    public async Task<bool> AddOrUpdateUserAsync(IUser user)
    {
        var existingUser = users.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser != null)
        {
            users.Remove(existingUser);
            users.Add(user);
            return (await SaveChangesInUserListAsync());
        }
        else
        {
            user.Id = users.Count + 1;
            users.Add(user);
            var result = await SaveChangesInUserListAsync();
            if (result && users.Contains(user))
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
