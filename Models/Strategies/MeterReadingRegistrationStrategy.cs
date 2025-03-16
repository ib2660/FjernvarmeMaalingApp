using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Models.Interfaces;
using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.ViewModels;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class MeterReadingRegistrationStrategy(ILogger<MeterReadingRegistrationStrategy> logger) : IRegistrationStrategy
{
    public string Name => "Måleraflæsning start/slut";
    public double ConsumptionInputValue { get; set; } = 0;
    private double _endValue = 0;
    private readonly ILogger<MeterReadingRegistrationStrategy> Logger = logger;
    public void Execute(Measurement measurement)
    {
        if (ConsumptionInputValue == 0 || _endValue == 0)
        {
            Logger.LogError("StartMeasure or EndMeasure is 0");
            return;
        }
        if (ConsumptionInputValue > _endValue)
        {
            Logger.LogError("StartMeasure is greater than EndMeasure");
            return;
        }
        measurement.Consumption = _endValue - ConsumptionInputValue;
        ConsumptionInputValue = 0;
        _endValue = 0;
    }
    public RenderFragment GetInputComponent()
    {
        return builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Indtast måleraflæsning start:");
            builder.OpenElement(2, "input");
            builder.AddAttribute(3, "type", "number");
            builder.AddAttribute(4, "step", "any");
            builder.AddAttribute(5, "value", ConsumptionInputValue);
            builder.AddAttribute(6, "oninput", EventCallback.Factory.CreateBinder<double>(this, value => ConsumptionInputValue = value, ConsumptionInputValue));
            builder.CloseElement();
            builder.AddContent(7, "Indtast måleraflæsning slut:");
            builder.OpenElement(8, "input");
            builder.AddAttribute(9, "type", "number");
            builder.AddAttribute(10, "step", "any");
            builder.AddAttribute(11, "value", _endValue);
            builder.AddAttribute(12, "oninput", EventCallback.Factory.CreateBinder<double>(this, value => _endValue = value, _endValue));
            builder.CloseElement();
            builder.CloseElement();
        };
    }
}
