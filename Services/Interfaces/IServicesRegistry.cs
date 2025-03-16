using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Interfaces;

namespace FjernvarmeMaalingApp.Services.Interfaces;
public interface IServicesRegistry
{
    IEnumerable<IRegistrationStrategy> GetAllRegistrationStrategies();
    IRegistrationStrategy? GetRegistrationStrategy(string name); 
    IEnumerable<ITimeFrameStrategy> GetAllTimeFrameStrategies();
    ITimeFrameStrategy? GetTimeFrameStrategy(string name); 
    IEnumerable<IMeasurementDisplayStrategy> GetAllMeasurementDisplayStrategies();
    IMeasurementDisplayStrategy? GetMeasurementDisplayStrategy(string name);
    IEnumerable<IConsumptionTypeFactory> GetAllConsumptionTypeFactories();
    IConsumptionTypeFactory? GetConsumptionTypeFactory(string name);
}
