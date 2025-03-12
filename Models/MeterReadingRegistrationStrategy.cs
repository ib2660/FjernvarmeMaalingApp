using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Models.Interfaces;
using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.ViewModels;

namespace FjernvarmeMaalingApp.Models;

public class MeterReadingRegistrationStrategy(ILogger<MeterReadingRegistrationStrategy> logger) : IRegistrationStrategy
{
    public string Name => "Måleraflæsning start/slut"; 
    private readonly ILogger<MeterReadingRegistrationStrategy> Logger = logger;
    
    public void Execute(Measurement measurement)
    {
        if (!measurement.StartMeasure.HasValue || !measurement.EndMeasure.HasValue)
        {
            Logger.LogError("StartMeasure or EndMeasure is null");
            return;
        }
        if (measurement.StartMeasure > measurement.EndMeasure)
        {
            Logger.LogError("StartMeasure is greater than EndMeasure");
            return;
        }
        measurement.Consumption = measurement.EndMeasure.Value - measurement.StartMeasure.Value;
    }

    public RenderFragment GetInputComponent(GemDataViewModel gemDataViewModel)
    {
        return builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Indtast måleraflæsning start:");
            builder.OpenElement(2, "input");
            builder.AddAttribute(3, "type", "number");
            builder.AddAttribute(4, "step", "any");
            builder.AddAttribute(5, "bind", "gemDataViewModel.Measurement.StartMeasure");
            builder.CloseElement();
            builder.AddContent(6, "Indtast måleraflæsning slut:");
            builder.OpenElement(7, "input");
            builder.AddAttribute(8, "type", "number");
            builder.AddAttribute(9, "step", "any");
            builder.AddAttribute(10, "bind", "gemDataViewModel.Measurement.EndMeasure");
            builder.CloseElement();
            builder.OpenComponent<SubmitMeasureButton>(11);
            builder.AddAttribute(12, "OnSubmit", EventCallback.Factory.Create(this, () => OnSubmit(gemDataViewModel)));
            builder.CloseComponent();
            builder.CloseElement();
        };
    }

    public void OnSubmit(GemDataViewModel gemDataViewModel)
    {
        gemDataViewModel.SendMeasurementData();
    }
}
