using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.ViewModels;

namespace FjernvarmeMaalingApp.Models.Interfaces;

public interface IRegistrationStrategy
{
    string Name { get; }
    void Execute(Measurement measurement);
    RenderFragment GetInputComponent(GemDataViewModel gemDataViewModel);
    void OnSubmit(GemDataViewModel gemDataViewModel);
}
