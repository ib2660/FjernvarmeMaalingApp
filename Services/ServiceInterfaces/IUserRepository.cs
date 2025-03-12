using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.Services.ServiceInterfaces;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username);
    Task<bool> AddUserAsync(User user);
}
