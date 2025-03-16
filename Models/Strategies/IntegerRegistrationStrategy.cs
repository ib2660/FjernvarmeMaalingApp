using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class IntegerRegistrationStrategy(ILogger<IntegerRegistrationStrategy> logger) : IRegistrationStrategy
{
    public string Name => "Faktisk forbrug";
    public double ConsumptionInputValue { get ; set ; } = 0;
    private readonly ILogger<IntegerRegistrationStrategy> Logger = logger;
    // TODO: Erstat ConsumptionInputValue i Measure med den lokale værdi i implementeringen. På den måde bliver registreringsstrategien uafhængig af hvilke parametre der er plads til i Measure. Der behøver kun at være en ConvertedConsumption
    public void Execute(Measurement measurement)
    {
        if (ConsumptionInputValue == 0)
        {
            Logger.LogError("Consumption is zero");
            return;
        }
        measurement.Consumption = ConsumptionInputValue;
    }

    public RenderFragment GetInputComponent()
    {
        return builder =>
        {
            builder.OpenElement(0, "div");
            builder.AddContent(1, "Indtast det faktiske forbrug:");
            builder.OpenElement(2, "input");
            builder.AddAttribute(3, "type", "number");
            builder.AddAttribute(4, "step", "any");
            builder.AddAttribute(5, "value", ConsumptionInputValue);
            builder.AddAttribute(6, "oninput", EventCallback.Factory.CreateBinder<double>(this, value => ConsumptionInputValue = value, ConsumptionInputValue));
            builder.CloseElement();
            builder.CloseElement();
        };
    }
}
    