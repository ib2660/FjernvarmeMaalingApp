using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username);
    Task<bool> AddOrUpdateUserAsync(User user);
}
