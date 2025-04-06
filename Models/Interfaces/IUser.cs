namespace FjernvarmeMaalingApp.Models.Interfaces;
public interface IUser
{
    int Id { get; set; }
    string Username { get; set; }
    string PreferredConsumptionTypeName { get; set; }
    string PreferredRegistrationStrategyName { get; set; }
    string PreferredTimeFrameStrategyName { get; set; }
    bool CheckPassword(string password);
    void UpdatePassword(string newPassword);
}
