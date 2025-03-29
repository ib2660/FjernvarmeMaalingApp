using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using FjernvarmeMaalingApp.Data.Interfaces;

namespace FjernvarmeMaalingApp.Models;
public class User
{
    [JsonPropertyName("Id")]
    public int Id { get; set; }
    
    [JsonPropertyName("Username")]
    public required string Username { get; set; }
    
    [JsonPropertyName("PasswordHash")]
    public required string PasswordHash { get; set; }
    
    [JsonPropertyName("Salt")]
    public required string Salt { get; set; }
    
    [JsonPropertyName("Preferred Consumption Type Name")]
    public string PreferredConsumptionTypeName { get; set; } = string.Empty;

    [JsonPropertyName("Preferred Registration Strategy Name")]
    public string PreferredRegistrationStrategyName { get; set; } = string.Empty;

    [JsonPropertyName("Preferred Time Frame")]
    public string PreferredTimeFrameStrategyName { get; set; } = string.Empty;
        
    private User() { } 

    // Constructor til at deserialize fra JSON
    [JsonConstructor]
    public User(int id, string username, string passwordHash, string salt, string preferredConsumptionTypeName, string preferredRegistrationStrategyName, string preferredTimeFrameStrategyName)
    {
        Id = id;
        Username = username;
        PasswordHash = passwordHash;
        Salt = salt;
        PreferredConsumptionTypeName = preferredConsumptionTypeName;
        PreferredRegistrationStrategyName = preferredRegistrationStrategyName;
        PreferredTimeFrameStrategyName = preferredTimeFrameStrategyName;
    }

    public static async Task<bool> CreateUserAsync(string username, string password, IUserRepository UserRepository, string preferredConsumptionTypeName, string preferredRegistrationStrategyName, string preferredTimeFrameStrategyName)
    {
        var salt = GenerateSalt();
        var passwordHash = HashPassword(password, salt);
        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash,
            Salt = salt,
            PreferredConsumptionTypeName = preferredConsumptionTypeName,
            PreferredRegistrationStrategyName = preferredRegistrationStrategyName,
            PreferredTimeFrameStrategyName = preferredTimeFrameStrategyName
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
    public override string ToString()
    {
        if (this != null)
        {
            return $"User: {Username}";
        }
        return "";
    }
}
