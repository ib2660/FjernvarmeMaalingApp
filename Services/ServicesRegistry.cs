using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Interfaces;

namespace FjernvarmeMaalingApp.Services;

public class ServicesRegistry : IServicesRegistry
{
    private readonly Dictionary<string, IRegistrationStrategy> _registrationStrategies;
    private readonly Dictionary<string, IMeasurementDisplayStrategy> _measurementDisplayStrategies;
    private readonly Dictionary<string, ITimeFrameStrategy> _timeFrameStrategies;
    private readonly Dictionary<string, IConsumptionTypeFactory> _consumptionTypeFactories;

    public ServicesRegistry(
   
        IEnumerable<IRegistrationStrategy> registrationStrategies,
        IEnumerable<IMeasurementDisplayStrategy> measurementDisplayStrategies,
        IEnumerable<ITimeFrameStrategy> timeFrameStrategies,
        IEnumerable<IConsumptionTypeFactory> consumptionTypeFactories)
    {
        _registrationStrategies = registrationStrategies.ToDictionary(s => s.Name, s => s);
        _measurementDisplayStrategies = measurementDisplayStrategies.ToDictionary(d => d.DisplayName, d => d);
        _timeFrameStrategies = timeFrameStrategies.ToDictionary(t => t.Name, t => t);
        _consumptionTypeFactories = consumptionTypeFactories.ToDictionary(c => c.Name, c => c);
    }

    public IEnumerable<IRegistrationStrategy> GetAllRegistrationStrategies()
        => _registrationStrategies.Values;

    public IRegistrationStrategy? GetRegistrationStrategy(string name)
        => _registrationStrategies.TryGetValue(name, out var registrationStrategy) ? registrationStrategy : null;

    public IEnumerable<IMeasurementDisplayStrategy> GetAllMeasurementDisplayStrategies()
        => _measurementDisplayStrategies.Values;

    public IMeasurementDisplayStrategy? GetMeasurementDisplayStrategy(string name)
        => _measurementDisplayStrategies.TryGetValue(name, out var measurementDisplayStrategy) ? measurementDisplayStrategy : null;

    public IEnumerable<ITimeFrameStrategy> GetAllTimeFrameStrategies()
        => _timeFrameStrategies.Values;
    public ITimeFrameStrategy? GetTimeFrameStrategy(string name)
        => _timeFrameStrategies.TryGetValue(name, out var timeFrameStrategy) ? timeFrameStrategy : null;

    public IEnumerable<IConsumptionTypeFactory> GetAllConsumptionTypeFactories()
        => _consumptionTypeFactories.Values;

    public IConsumptionTypeFactory? GetConsumptionTypeFactory(string name)
        => _consumptionTypeFactories.TryGetValue(name, out var consumptionTypeFactory) ? consumptionTypeFactory : null;
}
