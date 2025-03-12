namespace FjernvarmeMaalingApp.Models.Interfaces;

public interface IConsumptionType
{
    string ConsumptionTypeName { get; }
    double EnergyEquivalent {get; }
}