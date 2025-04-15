namespace FjernvarmeMaalingApp.Models.Interfaces
{
    public interface IUserFactory
    {
        Task<IUser> CreateUserAsync(string username, string password, string preferredConsumptionTypeName, string preferredRegistrationStrategyName, string preferredTimeFrameStrategyName);
    }
}
