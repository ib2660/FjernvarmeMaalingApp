using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.ViewModels;
public class OpretBrugerViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<User> _userLogger;
    private readonly ILogger<OpretBrugerViewModel> _logger;

    public UserModel RegisterModel { get; set; } = new UserModel();

    public OpretBrugerViewModel(IUserRepository userRepository, ILogger<User> userLogger, ILogger<OpretBrugerViewModel> logger)
    {
        _userRepository = userRepository;
        _userLogger = userLogger;
        _logger = logger;
    }

    public async Task HandleRegister()
    {
        RegisterModel.Response = string.Empty;

        if (!await User.CreateUserAsync(RegisterModel.Username!, RegisterModel.Password!, _userRepository, _userLogger))
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
