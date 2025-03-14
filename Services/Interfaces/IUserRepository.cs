using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.Services.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username);
    Task<bool> AddOrUpdateUserAsync(User user);
}
