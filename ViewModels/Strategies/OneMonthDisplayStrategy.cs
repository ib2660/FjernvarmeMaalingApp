using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class OneMonthDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis målinger for en måned";
    public void Execute(IEnumerable<Measurement> measurements)
    {
        // Implementer logik for at vise et sammendrag af målinger
    }
}
