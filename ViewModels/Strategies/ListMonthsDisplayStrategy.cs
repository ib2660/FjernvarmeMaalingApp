using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using System.Globalization;

namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class ListMonthsDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis liste af måneder";
    public void Execute(IEnumerable<Measurement> measurements)
    {
        var months = measurements
            .Where(m => m.MeasurementDate.HasValue)
            .Select(m => m.MeasurementDate!.Value.ToString("MMMM", CultureInfo.InvariantCulture))
            .Distinct();
    }
}
