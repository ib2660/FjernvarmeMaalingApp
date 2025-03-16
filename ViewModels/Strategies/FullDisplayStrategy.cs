using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class FullDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis alle m�linger";
    public void Execute(IEnumerable<Measurement> measurements)
    {
        // Implementer logik for at vise alle m�linger
    }
}
