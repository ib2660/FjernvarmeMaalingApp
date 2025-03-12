namespace FjernvarmeMaalingApp.Services.ServiceInterfaces;

public interface IWriteDataRepository
{
    bool EnterData(string json);
    bool UpdateData(string json);
}
