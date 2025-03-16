using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Runtime.ExceptionServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace FjernvarmeMaalingApp.ViewModels;

public class GemDataViewModel(ILogger<GemDataViewModel> logger, IUserRepository userRepository, IAuthenticationService authService, IServicesRegistry servicesRegistry, IWriteDataRepository writeDataRepository)
{
    private readonly ILogger<GemDataViewModel> _logger = logger;
    private readonly IWriteDataRepository _writeDataRepository = writeDataRepository;
    private readonly IServicesRegistry _servicesRegistry = servicesRegistry;
    private readonly IAuthenticationService _authService = authService;
    private readonly IUserRepository _userRepository = userRepository;
    public Measurement? Measurement { get; set; } = new Measurement();
    public Action? OnStateChange { get; set; }
    public string selectedTimeFrameName = string.Empty;
    public string selectedConsumptionTypeName = string.Empty;
    private string _selectedRegistrationStrategyName = string.Empty;
    public string SelectedRegistrationStrategyName
    {
        get => _selectedRegistrationStrategyName;
        set
        {
            if (_selectedRegistrationStrategyName != value)
            {
                _selectedRegistrationStrategyName = value;
                OnStateChange?.Invoke();
            }
        }
    }
    public User? CurrentUser {get; private set; }
    public async Task InitializeAsync()
    {
        await SetCurrentUser();
        if (CurrentUser != null)
        {
            selectedTimeFrameName = CurrentUser.PreferredTimeFrameStrategyName;
            selectedConsumptionTypeName = CurrentUser.PreferredConsumptionTypeName;
            _selectedRegistrationStrategyName = CurrentUser.PreferredRegistrationStrategyName;          
        }
        else
        {
            _logger.LogError("CurrentUser is null. Settings not updated.");
        }
    }
    public List<string> GetConsumptionTypeNames()
    {
        return _servicesRegistry.GetAllConsumptionTypeFactories().Select(f => f.Name).ToList();
    }
    public List<string> GetRegistrationStrategyNames()
    {
        return _servicesRegistry.GetAllRegistrationStrategies().Select(s => s.Name).ToList();
    }
    public IRegistrationStrategy GetRegistrationStrategy(string name)
    {
        return _servicesRegistry.GetRegistrationStrategy(name)!;
    }
    public List<string> GetTimeFrameStrategyNames()
    {
        // _logger.LogInformation("GetTimeFrameStrategyNames called");
        return _servicesRegistry.GetAllTimeFrameStrategies().Select(s => s.Name).ToList();
    }

    private void ExecuteRegistrationStrategy()
    {
        if (_selectedRegistrationStrategyName != string.Empty)
        {
            _servicesRegistry.GetRegistrationStrategy(_selectedRegistrationStrategyName)!.Execute(Measurement!);
        }
        else
        {
            _logger.LogError("RegistrationStrategy is null");
        }
    }

    private void ExecuteTimeFrameStrategy()
    {
        if (selectedTimeFrameName != string.Empty)
        {
            _servicesRegistry.GetTimeFrameStrategy(selectedTimeFrameName)!.Execute(Measurement!);
        }
        else
        {
            _logger.LogError("TimeFrameStrategy is null");
        }
    }

    private async Task Setdatafields()
    {
        Measurement!.RegisteredBy = CurrentUser!.Username;
        Measurement.TimeFrame = CurrentUser.PreferredTimeFrameStrategyName;
        Measurement.ConsumptionType = CurrentUser.PreferredConsumptionTypeName;
    }

    public async Task SubmitMeasurementData()
    {
        await SetCurrentUser();
        if (Measurement != null && CurrentUser != null)
        {
            Setdatafields();
            ExecuteRegistrationStrategy();
            ExecuteTimeFrameStrategy();
            if (Measurement.Validate(out var validationResults))
            {
                var json = JsonSerializer.Serialize(Measurement);
                var success = await _writeDataRepository.EnterData(json);
                if (!success)
                {
                    _logger.LogError("Failed to submit measurement data");
                }
                else
                {
                    _logger.LogInformation("Measurement saved");
                }
            }
            else
            {
                foreach (var validationResult in validationResults)
                {
                    _logger.LogError(validationResult.ErrorMessage);
                }
            }
        }
        else
        {
            _logger.LogError("Measurement is null");
        }
        Measurement = new Measurement();
        OnStateChange?.Invoke();
    }

    private async Task SetCurrentUser()
    {
        var claimsPrincipal = await _authService.GetCurrentUserAwait();
        var username = claimsPrincipal.Identity?.Name;
        if (username != null)
        {
            CurrentUser = await _userRepository.GetUserAsync(username);
        }
    }
}
