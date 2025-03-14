using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.ViewModels;

public class BrugerOpsætningViewModel
{
    public User? CurrentUser { get; private set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public string ResponseMessage { get; set; } = string.Empty;
    public string SelectedConsumptionType { get; set; } = string.Empty;
    public string SelectedRegistrationStrategy { get; set; } = string.Empty;

    public readonly Dictionary<string, IConsumptionTypeFactory> ConsumptionTypeFactories; // Dictionary som modtager ConsumptionType og ConsumptionTypeFactories fra Dependency Injection
    public readonly Dictionary<string, IRegistrationStrategy> RegistrationStrategies; // Dictionary som modtager RegistrationStrategy og RegistrationStrategies fra Dependency Injection
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;
    public IConsumptionType PreferredConsumptionType { get; private set; }
    public BrugerOpsætningViewModel(IEnumerable<IConsumptionTypeFactory> consumptionTypeFactories, IEnumerable<IRegistrationStrategy> registrationStrategies, IUserRepository userRepository, IAuthenticationService authService)
    {
        ConsumptionTypeFactories = consumptionTypeFactories.ToDictionary(ct => ct.ConsumptionTypeName);
        RegistrationStrategies = registrationStrategies.ToDictionary(rs => rs.Name);
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task InitializeAsync()
    {
        var claimsPrincipal = await _authService.GetCurrentUserAwait();
        var username = claimsPrincipal.Identity?.Name;
        if (username != null)
        {
            CurrentUser = await _userRepository.GetUserAsync(username);
        }
    }

    public async Task UpdateUserPasswordAsync() // TODO: denne giver en exception
    {
        if (NewPassword != ConfirmPassword)
        {
            ResponseMessage = "Passwords do not match.";
            return;
        }

        if (!string.IsNullOrEmpty(NewPassword))
        {
            User.UpdatePassword(NewPassword, CurrentUser!);
        }
        await _userRepository.AddOrUpdateUserAsync(CurrentUser!);
        ResponseMessage = "User updated.";
    }

    public async Task<bool> UpdateUserDetails()
    {
        if (CurrentUser == null)
        {
            return false;
        }
        if (!string.IsNullOrEmpty(SelectedConsumptionType) && ConsumptionTypeFactories.TryGetValue(SelectedConsumptionType, out var consumptionTypeFactory))
        {
            CurrentUser.PreferredConsumptionType = consumptionTypeFactory.CreateConsumptionType();
        }

        if (!string.IsNullOrEmpty(SelectedRegistrationStrategy) && RegistrationStrategies.TryGetValue(SelectedRegistrationStrategy, out var registrationStrategyInstance))
        {
            CurrentUser.PreferredRegistrationStrategy = registrationStrategyInstance;
        }
        return await _userRepository.AddOrUpdateUserAsync(CurrentUser!);
    }
}
