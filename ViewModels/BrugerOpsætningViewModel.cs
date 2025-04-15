using System.Security.Claims;
using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.ViewModels;

public class BrugerOpsætningViewModel
{
    public IUser? CurrentUser { get; private set; }
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
    public List<string> ResponseMessages { get; set; } = [string.Empty, string.Empty];
    public string SelectedConsumptionType { get; set; } = string.Empty;
    public string SelectedRegistrationStrategy { get; set; } = string.Empty;
    public string SelectedTimeFrameStrategy { get; set; } = string.Empty;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authService;
    private readonly IServicesRegistry _servicesRegistry;

    public BrugerOpsætningViewModel(IUserRepository userRepository, IAuthenticationService authService, IServicesRegistry servicesRegistry, IUserFactory userFactory)
    {
        _userRepository = userRepository;
        _authService = authService;
        _servicesRegistry = servicesRegistry;
    }

    public async Task InitializeAsync()
    {
        var claimsPrincipal = await _authService.GetCurrentUserAsync();
        var username = claimsPrincipal.Identity?.Name;
        if (username != null)
        {
            CurrentUser = await _userRepository.GetUserAsync(username);
        }
    }

    public async Task UpdateUserPasswordAsync() // TODO: denne giver en exception
    {
        ResponseMessages[0] = string.Empty;
        if (NewPassword != ConfirmPassword)
        {
            ResponseMessages[0] = "Passwords matcher ikke.";
            return;
        }

        if (!string.IsNullOrEmpty(NewPassword))
        {
            CurrentUser?.UpdatePassword(NewPassword);
        }
        if (await _userRepository.AddOrUpdateUserAsync((User)CurrentUser!))
        {
            ResponseMessages[0] = "Password blev opdateret.";
            return;
        }
        ResponseMessages[0] = "Fejl ved opdatering af password.";
    }

    public List<string> GetConsumptionTypeNames()
    {
        return _servicesRegistry.GetAllConsumptionTypeFactories().Select(f => f.Name).ToList();
    }

    public List<string> GetRegistrationStrategyNames()
    {
        return _servicesRegistry.GetAllRegistrationStrategies().Select(s => s.Name).ToList();
    }

    public List<string> GetTimeFrameStrategyNames()
    {
        return _servicesRegistry.GetAllTimeFrameStrategies().Select(s => s.Name).ToList();
    }

    public async Task<bool> UpdateUserDetailsAsync()
    {
        ResponseMessages[1] = string.Empty;
        if (CurrentUser == null)
        {
            ResponseMessages[1] = "Bruger opdatering fejlede. Ingen bruger registreret.";
            return false;
        }

        if (!string.IsNullOrEmpty(SelectedConsumptionType))
        {
            var consumptionTypeFactory = _servicesRegistry.GetConsumptionTypeFactory(SelectedConsumptionType);
            if (consumptionTypeFactory != null)
            {
                CurrentUser.PreferredConsumptionTypeName = SelectedConsumptionType;
                ResponseMessages[1] = "Bruger opdatering gennemført.";
            }
            else
            {
                ResponseMessages[1] = "Bruger opdatering fejlede vedrørende forbrugstype.";
                return false;
            }
        }

        if (!string.IsNullOrEmpty(SelectedRegistrationStrategy))
        {
            var registrationStrategyInstance = _servicesRegistry.GetRegistrationStrategy(SelectedRegistrationStrategy);
            if (registrationStrategyInstance != null)
            {
                CurrentUser.PreferredRegistrationStrategyName = SelectedRegistrationStrategy;
                ResponseMessages[1] = "Bruger opdatering gennemført.";
            }
            else
            {
                ResponseMessages[1] = "Bruger opdatering fejlede vedrørende registreringsstrategi.";
                return false;
            }
        }

        if (!string.IsNullOrEmpty(SelectedTimeFrameStrategy))
        {
            var timeFrameStrategyInstance = _servicesRegistry.GetTimeFrameStrategy(SelectedTimeFrameStrategy);
            if (timeFrameStrategyInstance != null)
            {
                CurrentUser.PreferredTimeFrameStrategyName = SelectedTimeFrameStrategy;
                ResponseMessages[1] = "Bruger opdatering gennemført.";
            }
            else
            {
                ResponseMessages[1] = "Bruger opdatering fejlede vedrørende tidsrammestrategi.";
                return false;
            }
        }
        return await _userRepository.AddOrUpdateUserAsync((User)CurrentUser!);
    }
}
