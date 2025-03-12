using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.Models;

public class IntegerRegistrationStrategy(ILogger<IntegerRegistrationStrategy> logger) : IRegistrationStrategy
{
    public string Name => "Faktisk forbrug"; 
    private readonly ILogger<IntegerRegistrationStrategy> Logger = logger;

    public void Execute(Measurement measurement)
    {
        if (measurement.Consumption == 0)
        {
            Logger.LogError("Consumption is zero");
            return;
        }
    }

    public RenderFragment GetInputComponent(GemDataViewModel gemDataViewModel)
    {
        return builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Indtast det faktiske forbrug:");
            builder.OpenElement(2, "input");
            builder.AddAttribute(3, "type", "number");
            builder.AddAttribute(4, "step", "any");
            builder.AddAttribute(5, "bind", "gemDataViewModel.Measurement.Consumption");
            builder.CloseElement();
            builder.OpenComponent<SubmitMeasureButton>(6);
            builder.AddAttribute(7, "OnSubmit", EventCallback.Factory.Create(this, () => OnSubmit(gemDataViewModel)));
            builder.CloseComponent();
            builder.CloseElement();
        };
    }

    public void OnSubmit(GemDataViewModel gemDataViewModel)
    {
        gemDataViewModel.SendMeasurementData();
    }
}
    