using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models;

public class FjernvarmeForbrugMWh : IConsumptionType
{
    public string ConsumptionTypeName { get; private set; } = "Fjernvarme i MWh";
    public double EnergyEquivalent { get; private set; } = 1.0;

    private FjernvarmeForbrugMWh() { }

    public static FjernvarmeForbrugMWh Instance { get; } = new FjernvarmeForbrugMWh();
}
