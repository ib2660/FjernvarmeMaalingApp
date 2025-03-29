using CommunityToolkit.Mvvm.ComponentModel;
using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.ViewModels;

public partial class GemDataViewModel(ILogger<GemDataViewModel> logger, IUserRepository userRepository, IAuthenticationService authService, IServicesRegistry servicesRegistry, IWriteDataRepository writeDataRepository) : ObservableObject
{
    private readonly ILogger<GemDataViewModel> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IAuthenticationService _authService = authService;
    private readonly IServicesRegistry _servicesRegistry = servicesRegistry;
    private readonly IWriteDataRepository _writeDataRepository = writeDataRepository;
    public Measurement? Measurement { get; set; } = new Measurement();
    public Action? OnStateChange { get; set; }
    [ObservableProperty] 
    public string selectedTimeFrameName = string.Empty;
    [ObservableProperty]
    public string selectedConsumptionTypeName = string.Empty;
    [ObservableProperty] 
    private string _selectedRegistrationStrategyName = string.Empty;
    public User? CurrentUser { get; private set; }
    public async Task InitializeAsync()
    {
        await SetCurrentUser();
        if (CurrentUser != null)
        {
            SelectedTimeFrameName = CurrentUser.PreferredTimeFrameStrategyName;
            SelectedConsumptionTypeName = CurrentUser.PreferredConsumptionTypeName;
            SelectedRegistrationStrategyName = CurrentUser.PreferredRegistrationStrategyName;
        }
        else
        {
            _logger.LogError("CurrentUser is null. Settings not updated.");
        }
    }

    private async Task SetCurrentUser()
    {
        CurrentUser = null;
        System.Security.Claims.ClaimsPrincipal claimsPrincipal = await _authService.GetCurrentUserAwait();
        string? username = claimsPrincipal.Identity?.Name;
        if (username != null)
        {
            CurrentUser = await _userRepository.GetUserAsync(username);
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
    private List<Measurement> ExecuteTimeFrameStrategy()
    {
        if (selectedTimeFrameName != string.Empty)
        {
            return _servicesRegistry.GetTimeFrameStrategy(selectedTimeFrameName)!.Execute(Measurement!);
        }
        else
        {
            _logger.LogError("TimeFrameStrategy is null");
            return [];
        }
    }
    private void Setdatafields()
    {
        Measurement!.RegisteredBy = CurrentUser!.Username;
        Measurement.TimeFrame = SelectedTimeFrameName;
        Measurement.ConsumptionType = SelectedConsumptionTypeName;
    }

    public async Task SubmitMeasurementData()
    {
        await SetCurrentUser();
        if (Measurement != null && CurrentUser != null)
        {
            Setdatafields();
            ExecuteRegistrationStrategy();
            List<Measurement> measurements = ExecuteTimeFrameStrategy();
            foreach (Measurement m in measurements)
            {
                bool success = await _writeDataRepository.EnterData(m);
                if (!success)
                {
                    _logger.LogError("Failed to submit measurement data");
                }
                else
                {
                    _logger.LogInformation("Measurement saved");
                }
            }
            
        }
        else
        {

            _logger.LogError("Measurement and/or User is null. User: {CurrentUser}.", CurrentUser != null ? CurrentUser.ToString() : "null");
        }
        Measurement = new Measurement();
        OnStateChange?.Invoke();
    }
}
