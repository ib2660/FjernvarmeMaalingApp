using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using FjernvarmeMaalingApp.Services.ServiceInterfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.ViewModels;

public class BrugerOpsætningViewModel
{
    public readonly Dictionary<string, IConsumptionTypeFactory> _consumptionTypeFactories; // Dictionary som modtager ConsumptionType og ConsumptionTypeFactories fra Dependency Injection
    public readonly Dictionary<string, IRegistrationStrategy> _registrationStrategies; // Dictionary som modtager RegistrationStrategy og RegistrationStrategies fra Dependency Injection
    public IConsumptionType PreferredConsumptionType { get; private set; }
    public BrugerOpsætningViewModel(IEnumerable<IConsumptionTypeFactory> consumptionTypeFactories, IEnumerable<IRegistrationStrategy> registrationStrategies)
    {
        _consumptionTypeFactories = consumptionTypeFactories.ToDictionary(ct => ct.ConsumptionTypeName);
        _registrationStrategies = registrationStrategies.ToDictionary(rs => rs.Name);
    }

    public void SetPreferredConsumptionType(IConsumptionType consumptionType)
    {
        PreferredConsumptionType = consumptionType;
    }

    public async Task InitializeAsync() { }
}
