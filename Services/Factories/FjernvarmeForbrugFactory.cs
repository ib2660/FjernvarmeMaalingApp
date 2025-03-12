using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Factories;

public class FjernvarmeForbrugFactory : IConsumptionTypeFactory
{
    public string ConsumptionTypeName { get; private set;  }= "Fjernvarmeforbrug i MWh";
    public IConsumptionType CreateConsumptionType()
    {
        return FjernvarmeForbrugMWh.Instance;
    }
}
