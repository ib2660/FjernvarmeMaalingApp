using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Services;

public class WriteDataRepository(ILogger<WriteDataRepository> logger, IReadDataRepository readDataRepository, IAuthenticationService authenticationService, IUserRepository userRepository) : IWriteDataRepository
{
    private readonly ILogger<WriteDataRepository> _logger = logger;
    private readonly IReadDataRepository _readDataRepository = readDataRepository;
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly string _filePath = "C:/temp/data.json";
    private readonly Dictionary<string, Measurement> _cleaned = [];
    public async Task<bool> EnterData(List<Measurement> measurements)
    {
        string? userName = _authenticationService.GetCurrentUserAwait().Result.Identity?.Name;
        if (userName == null)
        {
            _logger.LogError("User is not authenticated");
            return false;
        }
        User? user = await _userRepository.GetUserAsync(userName);
        if (user == null)
        {
            _logger.LogError("User not found");
            return false;
        }
        string userData = await _readDataRepository.ReadData(user);
        string allData = await _readDataRepository.ReadData(null); // TODO: hvordan henter jeg alle data fra samme metode?
        if (userData == string.Empty)
        {
            _logger.LogWarning("No data found for user");
        }
        else
        {
            measurements.AddRange(JsonHelper.DeserializeObject<List<Measurement>>(userData));
            measurements.Reverse();
            foreach (Measurement measurement in measurements)
            {
                _cleaned[measurement.MeasurementDate.ToString()!] = measurement;
            }
            measurements.Clear();
            foreach (Measurement measurement in _cleaned.Values)
            {
                measurements.Add(measurement);
            }
        }
        string json = JsonHelper.SerializeObject(measurements);
        try
        {
            using FileStream stream = new(_filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            using StreamWriter writer = new(stream);
            await writer.WriteLineAsync(json);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while entering data: " + ex.Message);
            return false;
        }
    }
}
