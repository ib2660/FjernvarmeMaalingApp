using FjernvarmeMaalingApp.Models;
namespace FjernvarmeMaalingApp.Data.Interfaces;
public interface IWriteDataRepository
{
    Task <bool> EnterData(Measurement measurement);
}
