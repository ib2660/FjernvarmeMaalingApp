using FjernvarmeMaalingApp.Models;

namespace FjernvarmeMaalingApp.ViewModels.Interfaces;
public interface IMeasurementDisplayStrategy
{
    void Execute(IEnumerable<Measurement> measurements);
}
