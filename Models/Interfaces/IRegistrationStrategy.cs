using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.Models.Interfaces;

public interface IRegistrationStrategy
{
    string Name { get; }
    double ConsumptionInputValue { get; set; }
    RenderFragment GetInputComponent();
    void Execute(Measurement measurement);
}
