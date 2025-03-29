using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.Data.Interfaces;
public interface IReadDataRepository
{
    Task <string> ReadData(User? user);
}
