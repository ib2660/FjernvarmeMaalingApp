using System.Security.Cryptography;
using System.Text.Json.Serialization;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Models;

public class User
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    [JsonPropertyName("passwordHash")]
    public required string PasswordHash { get; set; }
    [JsonPropertyName("salt")]
    public required string Salt { get; set; }
    [JsonPropertyName("preferredConsumptionTypeName")]
    public string PreferredConsumptionTypeName { get; private set; }
    [JsonPropertyName("preferredRegistrationStrategyName")]
    public string PreferredRegistrationStrategyName { get; private set; }
    [JsonIgnore]
    public IConsumptionType? PreferredConsumptionType { get; set; }
    [JsonIgnore]
    public IRegistrationStrategy? PreferredRegistrationStrategy { get; set; }
    [JsonIgnore]
    private ILogger<User> Logger { get; set; }

    public User() { } // TODO: sæt til private efter test

    // Constructor til at deserialize fra JSON
    [JsonConstructor]
    public User(int id, string username, string passwordHash, string salt, string preferredConsumptionTypeName, string preferredRegistrationStrategyName)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Salt = salt;
        PreferredConsumptionTypeName = preferredConsumptionTypeName;
        PreferredRegistrationStrategyName = preferredRegistrationStrategyName;
    }

    public static async Task<bool> CreateUserAsync(string username, string password, IUserRepository UserRepository, ILogger<User> logger)
    {
        var salt = GenerateSalt();
        var passwordHash = HashPassword(password, salt);
        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Salt = salt,
            Logger = logger
        };
        if (await UserRepository.AddOrUpdateUserAsync(user))
        {
            return true;
        }
        return false;
    }

    public static void UpdatePassword(string newPassword, User user)
    {
        var salt = GenerateSalt();
        user.PasswordHash = HashPassword(newPassword, salt);
        user.Salt = salt;
        user.Logger.LogInformation($"User {user.Username} updated password.");
    }

    public static string CheckPassword(string password, string userSalt)
    {
        return HashPassword(password, userSalt);
    }

    private static string GenerateSalt()
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        return Convert.ToBase64String(salt);
    }

    private static string HashPassword(string password, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        using Rfc2898DeriveBytes pbkdf2 = new(password, saltBytes, 10000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(20);
        return Convert.ToBase64String(hash);
    }
}
