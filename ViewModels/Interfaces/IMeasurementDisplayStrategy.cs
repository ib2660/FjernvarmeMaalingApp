using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.ViewModels.Interfaces;
public interface IMeasurementDisplayStrategy
{
    string DisplayName { get; }
    void Execute(IEnumerable<Measurement> measurements);
}
