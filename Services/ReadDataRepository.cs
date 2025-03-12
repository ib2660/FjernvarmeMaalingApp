using FjernvarmeMaalingApp.Services.ServiceInterfaces;

namespace FjernvarmeMaalingApp.Services;

public class ReadDataRepository : IReadDataRepository
{
    public string ReadData()
    {
        throw new NotImplementedException(); //Implement strategies to handle concurrency issues, such as optimistic concurrency control, especially for write operations.
    }
}
