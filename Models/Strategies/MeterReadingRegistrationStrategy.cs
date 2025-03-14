using FjernvarmeMaalingApp.Components;
using FjernvarmeMaalingApp.Models.Interfaces;
using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.ViewModels;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class MeterReadingRegistrationStrategy(ILogger<MeterReadingRegistrationStrategy> logger) : IRegistrationStrategy
{
    public string Name => "Måleraflæsning start/slut"; 
    private readonly ILogger<MeterReadingRegistrationStrategy> Logger = logger;
    // TODO: Erstat start og slut i Measure med de lokale værdier i implementeringen. På den måde bliver registreringsstrategien uafhængig af hvilke parametre der er plads til i Measure. Der behøver kun at være en ConvertedConsumption.
    private double StartMeasure = 0;
    private double EndMeasure = 0;
    
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
        measurement.Consumption = measurement.EndMeasure - measurement.StartMeasure;
        measurement.StartMeasure = 0;
        measurement.EndMeasure = 0;
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
            builder.AddAttribute(5, "value", gemDataViewModel.Measurement!.StartMeasure);
            builder.AddAttribute(6, "oninput", EventCallback.Factory.CreateBinder<double?>(this, value => gemDataViewModel.Measurement.StartMeasure = value, gemDataViewModel.Measurement.StartMeasure));
            builder.CloseElement();
            builder.AddContent(7, "Indtast måleraflæsning slut:");
            builder.OpenElement(8, "input");
            builder.AddAttribute(9, "type", "number");
            builder.AddAttribute(10, "step", "any");
            builder.AddAttribute(11, "value", gemDataViewModel.Measurement.EndMeasure);
            builder.AddAttribute(12, "oninput", EventCallback.Factory.CreateBinder<double?>(this, value => gemDataViewModel.Measurement.EndMeasure = value, gemDataViewModel.Measurement.EndMeasure));
            builder.CloseElement();
            builder.OpenComponent<SubmitMeasureButton>(13);
            builder.AddAttribute(14, "OnSubmit", EventCallback.Factory.Create(this, () => OnSubmit(gemDataViewModel)));
            builder.CloseComponent();
            builder.CloseElement();
        };
    }


    public void OnSubmit(GemDataViewModel gemDataViewModel)
    {
        // TODO: foretag en validering af data, inden de skrives til datarepository.
        gemDataViewModel.SendMeasurementData();
    }
}
