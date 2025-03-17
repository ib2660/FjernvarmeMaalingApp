using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Services;

public class ReadDataRepository(ILogger<ReadDataRepository> logger, IAuthenticationService authenticationService, IUserRepository userRepository) : IReadDataRepository
{
    private readonly ILogger<ReadDataRepository> _logger = logger;
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly string _filePath = "C:/temp/data.json";

    public async Task<string> ReadData(User? user)
    {
        try
        {
            using (FileStream fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    string json = await sr.ReadToEndAsync();
                    var allMeasurements = JsonHelper.DeserializeObject<List<Measurement>>(json);

                    if (allMeasurements == null || !allMeasurements.Any())
                    {
                        _logger.LogWarning("No data found in the file");
                        return string.Empty;
                    }
                    if (user != null)
                    {
                        var userMeasurements = allMeasurements.Where(m => m.RegisteredBy == user.Username).ToList();
                        if (!userMeasurements.Any())
                        {
                            _logger.LogWarning("No data found for user");
                            return string.Empty;
                        }
                        return JsonHelper.SerializeObject(userMeasurements);
                    }
                    else
                    {
                        return JsonHelper.SerializeObject(allMeasurements);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reading data");
            return string.Empty;
        }
    }
}
