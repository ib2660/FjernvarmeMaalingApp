namespace FjernvarmeMaalingApp.Services.Interfaces;

public interface IWriteDataRepository
{
    bool EnterData(string json);
    bool UpdateData(string json);
}
