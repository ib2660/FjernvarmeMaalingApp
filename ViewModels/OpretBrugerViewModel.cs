using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.ServiceInterfaces;

namespace FjernvarmeMaalingApp.ViewModels;
public class OpretBrugerViewModel
{
    private readonly IUserRepository _userRepository;

    public UserModel RegisterModel { get; set; } = new UserModel();

    public OpretBrugerViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task HandleRegister()
    {
        RegisterModel.Response = string.Empty;
        if (!await User.CreateUserAsync(RegisterModel.Username!, RegisterModel.Password!, _userRepository))
        {
            RegisterModel.Response = "Brugeren blev ikke oprettet.";
        }
        else RegisterModel.Response = "Brugeren er oprettet";
        RegisterModel.ResetInstance();
    }

    public void AdjustComment(ChangeEventArgs e)
    {
        RegisterModel.Response = "";
    }
}
