using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models;

public class GasForbrugM3 : IConsumptionType
{
    public string ConsumptionTypeName { get; private set; } = "Gasforbrug i m3";
    public double EnergyEquivalent { get; private set; } = 0.01055;

    private GasForbrugM3() { }

    public static GasForbrugM3 Instance { get; } = new GasForbrugM3();
}
