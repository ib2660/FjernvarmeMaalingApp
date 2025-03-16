using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class SameMonthAllYearsDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis samme måned i alle år";

    public void Execute(IEnumerable<Measurement> measurements)
    {
        // Implementer logik for at vise samme måned over flere år
    }
}
