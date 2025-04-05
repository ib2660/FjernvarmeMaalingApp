using FjernvarmeMaalingApp.Models;
using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.ViewModels.Interfaces;
public interface IMeasurementDisplayStrategy
{
    string DisplayName { get; }
    RenderFragment Execute(IEnumerable<Measurement> measurements, EventCallback<IEnumerable<Measurement>> eventCallback);
    void Reset();
}