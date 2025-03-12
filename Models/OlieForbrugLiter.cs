using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models;

public class OlieForbrugLiter : IConsumptionType
{
    public string ConsumptionTypeName { get; private set; } = "Olieforbrug i liter";
    public double EnergyEquivalent { get; private set; } = 0.00959091;

    private OlieForbrugLiter() { }

    public static OlieForbrugLiter Instance { get; } = new OlieForbrugLiter();
}
