using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Factories;

public class GasForbrugFactory : IConsumptionTypeFactory
{
    public string Name { get; private set; } = "Gasforbrug i m3";
    public IConsumptionType CreateConsumptionType()
    {
        return GasForbrugM3.Instance;
    }
}
