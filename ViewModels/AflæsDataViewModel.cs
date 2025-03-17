using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using System.Text.Json;

namespace FjernvarmeMaalingApp.ViewModels;

public class AflæsDataViewModel(IAuthenticationService authenticationService, IUserRepository userRepository, IReadDataRepository readDataRepository, ILogger<AflæsDataViewModel> logger)
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IReadDataRepository _readDataRepository = readDataRepository;
    private readonly ILogger<AflæsDataViewModel> _logger = logger;
    private IMeasurementDisplayStrategy? _measurementDisplayStrategy;
    public User? CurrentUser { get; private set; }

    public async Task InitializeAsync()
    {
        var claimsPrincipal = await _authenticationService.GetCurrentUserAwait();
        var username = claimsPrincipal.Identity?.Name;
        if (username != null)
        {
            CurrentUser = await _userRepository.GetUserAsync(username);
        }
    }

    public async Task<IEnumerable<Measurement>?> GetMeasurementsAsync()
    {
        var result = new List<Measurement>();
        if (CurrentUser != null)
        {
            string json = await _readDataRepository.ReadData(CurrentUser);
            if (json != string.Empty)
            {
                result = JsonHelper.DeserializeObject<List<Measurement>>(json);
            }
        }
        return result;
    }

    public void SetDisplayStrategy(IMeasurementDisplayStrategy measurementDisplayStrategy)
    {
        _measurementDisplayStrategy = measurementDisplayStrategy;
        _logger.LogInformation($"Display strategy set to {0}", measurementDisplayStrategy.GetType().Name);
    }

    public void ExecuteDisplayStrategy(IEnumerable<Measurement> measurements)
    {
        if (_measurementDisplayStrategy == null)
        {
            return;
        }
        _measurementDisplayStrategy.Execute(measurements);
    }
}
