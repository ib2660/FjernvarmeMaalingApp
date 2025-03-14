using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.Services.Interfaces;
public interface IReadDataRepository
{
    Task <string> ReadData(User user);
}
