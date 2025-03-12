using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Factories;

public class OlieForbrugFactory : IConsumptionTypeFactory
{
    public string ConsumptionTypeName { get; private set; } = "Olieforbrug i liter";
    public IConsumptionType CreateConsumptionType()
    {
        return OlieForbrugLiter.Instance;
    }
}
