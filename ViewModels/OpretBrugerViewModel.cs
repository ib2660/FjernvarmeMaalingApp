using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.Models;
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

    public UserModel RegisterModel { get; set; } = new UserModel();

    public OpretBrugerViewModel(IUserRepository userRepository, ILogger<User> userLogger, ILogger<OpretBrugerViewModel> logger, IServicesRegistry servicesRegistry)
    {
        _userRepository = userRepository;
        _logger = logger;
        _servicesRegistry = servicesRegistry;
    }

    public async Task HandleRegister()
    {
        UserFactory userFactory = new(_userRepository); // TODO: læg denne i Program.cs som Singleton. Den skal ikke skrive nogen data, så den behøver ikke at være scoped.
        RegisterModel.Response = string.Empty;
        string c = _servicesRegistry.GetAllConsumptionTypeFactories().First().Name;
        string r = _servicesRegistry.GetAllRegistrationStrategies().First().Name;
        string t = _servicesRegistry.GetAllTimeFrameStrategies().First().Name;
        try
        {
            var newUser = await userFactory.CreateUserAsync(RegisterModel.Username!, RegisterModel.Password!, c, r, t);
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
