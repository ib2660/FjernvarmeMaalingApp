using FjernvarmeMaalingApp.Models;
namespace FjernvarmeMaalingApp.Services.Interfaces;
public interface IWriteDataRepository
{
    Task <bool> EnterData(List<Measurement> measurements);
}
