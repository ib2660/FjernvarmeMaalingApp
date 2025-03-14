using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Services;

public class ReadDataRepository : IReadDataRepository
{
    public async Task<string> ReadData(User user)
    {
        if (user == null)
        {
            return string.Empty;
        }

        // Read data from the file. Make sure only data from the user is read.
        string json = "Data read successfully.";

        return json;
    }
}
