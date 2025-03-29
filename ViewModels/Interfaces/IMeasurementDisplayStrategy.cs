using FjernvarmeMaalingApp.Models;
using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.ViewModels.Interfaces;
public interface IMeasurementDisplayStrategy
{
    string DisplayName { get; }
    void Execute(IEnumerable<Measurement> measurements, RenderFragment renderFragment);
}