using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using Microsoft.AspNetCore.Components;
namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class FullDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis alle m�linger";
    public void Execute(IEnumerable<Measurement> measurements)
    {
        // Nothing done as all items must be shown
    }
}