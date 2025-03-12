using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.ServiceInterfaces;

namespace FjernvarmeMaalingApp.ViewModels;
public class LoginViewModel
{
    private readonly IAuthenticationService _authProvider;
    private readonly NavigationManager _navigationManager;

    public event Action? StateChanged;
    private void NotifyStateChanged() => StateChanged?.Invoke();

    public UserModel LoginModel { get; set; } = new UserModel();
    public ClaimsPrincipal? CurrentUser { get; private set; }

    public LoginViewModel(IAuthenticationService authProvider, NavigationManager navigationManager)
    {
        _authProvider = authProvider;
        _navigationManager = navigationManager;
    }

    public async Task InitializeAsync()
    {
        CurrentUser = await _authProvider.GetCurrentUserAwait();
        LoginModel.Username = string.Empty;
        LoginModel.Password = string.Empty;
    }

    public async Task HandleLoginAsync()
    {
        LoginModel.Response = string.Empty;
        bool isValid = await _authProvider.ValidateLoginAsync(LoginModel.Username!, LoginModel.Password!);
        if (isValid)
        {
            CurrentUser = await _authProvider.GetCurrentUserAwait();
            _navigationManager.NavigateTo("/");
        }
        else
        {
            LoginModel.Response = "Brugeren blev ikke logget ind.";
            LoginModel.ResetInstance();
            NotifyStateChanged();
        }
    }

    public async Task LogOutAsync()
    {
        _authProvider.MarkUserAsLoggedOutAsync();
        CurrentUser = await _authProvider.GetCurrentUserAwait();
    }
}
