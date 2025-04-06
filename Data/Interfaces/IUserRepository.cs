using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Data.Interfaces;
public interface IUserRepository
{
    Task<IUser?> GetUserAsync(string username);
    Task<bool> AddOrUpdateUserAsync(IUser user);
}