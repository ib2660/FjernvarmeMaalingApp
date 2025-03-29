using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.Data.Interfaces;

namespace FjernvarmeMaalingApp.Services;

public class AuthenticationService(IUserRepository userRepository) : AuthenticationStateProvider, IAuthenticationService
{
    private readonly IUserRepository _userRepository = userRepository;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return new AuthenticationState(_currentUser);
    }

    public async Task<bool> ValidateLoginAsync(string usernameToValidate, string passwordToValidate)
    {
        var storedUser = await _userRepository.GetUserAsync(usernameToValidate);
        if (storedUser != null && storedUser.PasswordHash == User.CheckPassword(passwordToValidate, storedUser.Salt))
        {
            await MarkUserAsAuthenticatedAsync(storedUser.Username);
            return true;
        }
        return false;
    }

    public async Task<ClaimsPrincipal> GetCurrentUserAwait()
    {
        return _currentUser;
    }

    public async Task MarkUserAsAuthenticatedAsync(string username)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, username)
        }, "apiauth_type");

        _currentUser = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
        return;
    }

    public async Task MarkUserAsLoggedOutAsync()
    {
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
    }
}