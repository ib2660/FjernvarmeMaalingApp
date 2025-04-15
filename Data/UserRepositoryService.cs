using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
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
            if (!CheckUserFileExists()) { throw new Exception("No user file created."); }
            users = LoadUsers();
            _logger.LogInformation("Users loaded from JSON file.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize users from JSON file.");
            users = [];
        }
    }

    private List<User> LoadUsers()
    {
        string json = File.ReadAllText(_filePath);
        var u = JsonHelper.DeserializeObject<List<User>>(json) ?? [];
        if (u == null || u.Count == 0)
        {
            throw new Exception("null or empty error");
        }
        return u;
    }

    public async Task<IUser?> GetUserAsync(string username)
    {
        IUser? foundUser = users.FirstOrDefault(u => u.Username == username);
        return foundUser;
    }
    private async Task<bool> SaveChangesInUserListAsync()
    {
        if (CheckUserFileExists())
        {
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
            }
        }
        return false;
    }

    private bool CheckUserFileExists()
    {
        if (!File.Exists(_filePath))
        {
            try
            {
                _ = File.Create(_filePath);
                _logger.LogInformation("User list file created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create user list file.");
                return false;
            }
        }

        return true;
    }

    public async Task<bool> AddOrUpdateUserAsync(IUser user)
    {
        User? existingUser = users.FirstOrDefault(u => u.Username == user.Username);
        if (existingUser != null)
        {
            _ = users.Remove(existingUser);
            users.Add((User)user);
            return await SaveChangesInUserListAsync();
        }
        else
        {
            user.Id = users.Count + 1;
            users.Add((User)user);
            bool result = await SaveChangesInUserListAsync();
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
