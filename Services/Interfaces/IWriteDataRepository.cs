namespace FjernvarmeMaalingApp.Services.Interfaces;

public interface IWriteDataRepository
{
    Task <bool> EnterData(string json);
    Task <bool> UpdateData(string json);
}
