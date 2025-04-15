using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using static FjernvarmeMaalingApp.Models.User;

namespace FjernvarmeMaalingApp.ViewModels;
public class OpretBrugerViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<OpretBrugerViewModel> _logger;
    private readonly IServicesRegistry _servicesRegistry;
    private readonly IUserFactory _userFactory;

    public UserModel RegisterModel { get; set; } = new UserModel();

    public OpretBrugerViewModel(IUserRepository userRepository, ILogger<User> userLogger, ILogger<OpretBrugerViewModel> logger, IServicesRegistry servicesRegistry, IUserFactory userFactory)
    {
        _userRepository = userRepository;
        _logger = logger;
        _servicesRegistry = servicesRegistry;
        _userFactory = userFactory;
    }

    public async Task HandleRegister()
    {
        RegisterModel.Response = string.Empty;
        string c = _servicesRegistry.GetAllConsumptionTypeFactories().First().Name;
        string r = _servicesRegistry.GetAllRegistrationStrategies().First().Name;
        string t = _servicesRegistry.GetAllTimeFrameStrategies().First().Name;
        try
        {
            var newUser = await _userFactory.CreateUserAsync(RegisterModel.Username!, RegisterModel.Password!, c, r, t);
            RegisterModel.Response = "Brugeren er oprettet";
        }
        catch (Exception ex) {
            RegisterModel.Response = "Brugeren blev ikke oprettet.";
        }
        RegisterModel.ResetInstance();
    }

    public void AdjustComment(ChangeEventArgs e)
    {
        RegisterModel.Response = "";
    }
}
