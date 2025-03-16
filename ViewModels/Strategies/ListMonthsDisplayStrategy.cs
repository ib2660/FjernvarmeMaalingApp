using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;

namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class ListMonthsDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis liste af måneder";
    public void Execute(IEnumerable<Measurement> measurements)
    {
        // Implementer logik for at vise en liste af måneder
    }
}
