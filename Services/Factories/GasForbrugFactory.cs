using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Factories;

public class GasForbrugFactory : IConsumptionTypeFactory
{
    public string Name { get; private set; } = GasForbrugM3.Instance.ConsumptionTypeName;
    public IConsumptionType CreateConsumptionType()
    {
        return GasForbrugM3.Instance;
    }
}
