using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.ViewModels;
public class OpretBrugerViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OpretBrugerViewModel> _logger;
    private readonly IServicesRegistry _servicesRegistry;

    public UserModel RegisterModel { get; set; } = new UserModel();

    public OpretBrugerViewModel(IUserRepository userRepository, ILogger<User> userLogger, ILogger<OpretBrugerViewModel> logger, IServicesRegistry servicesRegistry)
    {
        _userRepository = userRepository;
        _logger = logger;
        _servicesRegistry = servicesRegistry;
    }

    public async Task HandleRegister()
    {
        RegisterModel.Response = string.Empty;
        string c = _servicesRegistry.GetAllConsumptionTypeFactories().First().Name;
        string r = _servicesRegistry.GetAllRegistrationStrategies().First().Name;
        string t = _servicesRegistry.GetAllTimeFrameStrategies().First().Name;
        if (!await User.CreateUserAsync(RegisterModel.Username!, RegisterModel.Password!, _userRepository, c, r, t))
        {
            RegisterModel.Response = "Brugeren blev ikke oprettet.";
        }
        else
        {
            RegisterModel.Response = "Brugeren er oprettet";
        }
        RegisterModel.ResetInstance();
    }

    public void AdjustComment(ChangeEventArgs e)
    {
        RegisterModel.Response = "";
    }
}
