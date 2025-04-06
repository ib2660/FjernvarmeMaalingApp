using System.Security.Cryptography;
using System.Text.Json.Serialization;
using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models;
public class User : IUser
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
    public string PreferredConsumptionTypeName { get; set; }

    [JsonPropertyName("Preferred Registration Strategy Name")]
    public string PreferredRegistrationStrategyName { get; set; }

    [JsonPropertyName("Preferred Time Frame")]
    public string PreferredTimeFrameStrategyName { get; set; }
        
    private User() { } 

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

    public interface IUserFactory
    {
        Task<IUser> CreateUserAsync(string username, string password, string preferredConsumptionTypeName, string preferredRegistrationStrategyName, string preferredTimeFrameStrategyName);
    }

    public class UserFactory : IUserFactory
    {
        private readonly IUserRepository _userRepository;
        public UserFactory(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IUser> CreateUserAsync(string username, string password, string preferredConsumptionTypeName, string preferredRegistrationStrategyName, string preferredTimeFrameStrategyName)
        {
            User newUser = new()
            {
                Username = username,
                PasswordHash = string.Empty,
                Salt = GenerateSalt(),
                PreferredConsumptionTypeName = preferredConsumptionTypeName,
                PreferredRegistrationStrategyName = preferredRegistrationStrategyName,
                PreferredTimeFrameStrategyName = preferredTimeFrameStrategyName
            };
            newUser.PasswordHash = newUser.HashPassword(password);
            if (await _userRepository.AddOrUpdateUserAsync(newUser))
            {
                return newUser;
            }
            throw new Exception("User creation failed.");
        }
    }

    public void UpdatePassword(string newPassword)
    {
        Salt = GenerateSalt();
        PasswordHash = HashPassword(newPassword);
    }

    public bool CheckPassword(string password)
    {
        return Salt == HashPassword(password);
    }

    private static string GenerateSalt()
    {
        byte[] salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        return Convert.ToBase64String(salt);
    }

    private string HashPassword(string newPassword)
    {
        byte[] saltBytes = Convert.FromBase64String(Salt);
        using Rfc2898DeriveBytes pbkdf2 = new(newPassword, saltBytes, 10000, HashAlgorithmName.SHA256);
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
