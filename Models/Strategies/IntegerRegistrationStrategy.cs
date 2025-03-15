using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.ViewModels;
using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class IntegerRegistrationStrategy(ILogger<IntegerRegistrationStrategy> logger) : IRegistrationStrategy
{
    public string Name => "Faktisk forbrug"; 
    private readonly ILogger<IntegerRegistrationStrategy> Logger = logger;
    // TODO: Erstat Consumption i Measure med den lokale værdi i implementeringen. På den måde bliver registreringsstrategien uafhængig af hvilke parametre der er plads til i Measure. Der behøver kun at være en ConvertedConsumption
    private double Consumption = 0;
    public void Execute(Measurement measurement)
    {
        if (measurement.Consumption == 0)
        {
            Logger.LogError("Consumption is zero");
            return;
        }
        measurement.StartMeasure = 0;
        measurement.EndMeasure = 0;
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
            builder.AddAttribute(5, "value", gemDataViewModel.Measurement!.Consumption);
            builder.AddAttribute(6, "oninput", EventCallback.Factory.CreateBinder<double?>(this, value => gemDataViewModel.Measurement.Consumption = value, gemDataViewModel.Measurement.Consumption));
            builder.CloseElement();
            //builder.OpenComponent<SubmitMeasureButton>(7);
            //builder.AddAttribute(8, "OnSubmit", EventCallback.Factory.Create(this, () => OnSubmit(gemDataViewModel)));
            //builder.CloseComponent();
            builder.CloseElement();
        };
    }


    public void OnSubmit(GemDataViewModel gemDataViewModel)
    {
        // TODO: foretag en validering af data, inden de skrives til datarepository.
        gemDataViewModel.SendMeasurementData();
    }
}
    