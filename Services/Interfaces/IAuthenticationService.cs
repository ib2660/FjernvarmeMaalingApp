using System.Security.Claims;

namespace FjernvarmeMaalingApp.Services.Interfaces;
public interface IAuthenticationService
{
    Task<bool> ValidateLoginAsync(string username, string password);
    Task<ClaimsPrincipal> GetCurrentUserAwait();
    Task MarkUserAsAuthenticatedAsync(string username);
    Task MarkUserAsLoggedOutAsync();
}