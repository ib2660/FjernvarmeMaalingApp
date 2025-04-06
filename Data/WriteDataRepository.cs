using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;
using System.Security.Claims;

namespace FjernvarmeMaalingApp.Data;

public class WriteDataRepository(ILogger<WriteDataRepository> logger, IAuthenticationService authenticationService, IUserRepository userRepository) : IWriteDataRepository
{
    private readonly ILogger<WriteDataRepository> _logger = logger;
        private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly string _filePath = "C:/temp/data.json";
        public async Task<bool> EnterData(Measurement measurement)
    {
        ClaimsPrincipal currentUser = await _authenticationService.GetCurrentUserAsync();
        string? userName = currentUser.Identity?.Name;
        if (userName == null)
        {
            _logger.LogError("User is not authenticated");
            return false;
        }
        User? user = (User?) await _userRepository.GetUserAsync(userName);
        if (user == null)
        {
            _logger.LogError("User not found");
            return false;
        }

        string json = JsonHelper.SerializeObject(measurement) + ",";
        try
        {
            using FileStream stream = new(_filePath, FileMode.Append, FileAccess.Write, FileShare.None);
            using StreamWriter writer = new(stream);
            await writer.WriteAsync(json);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred while entering data: {message}", ex.Message);
            return false;
        }
    }
}
