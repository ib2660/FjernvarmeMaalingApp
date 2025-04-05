using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using Microsoft.AspNetCore.Components;
using FjernvarmeMaalingApp.ViewModels.Strategies.StrategyComponents;

namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class FullDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis alle målinger";
    public RenderFragment Execute(IEnumerable<Measurement> measurements, EventCallback<IEnumerable<Measurement>> eventCallback)
    {
        RenderFragment renderFragment = builder =>
        {
            builder.OpenComponent<DisplayComponent>(0);
            builder.AddAttribute(1, "Measurements", measurements);
            builder.CloseComponent();
        };
        return renderFragment;
    }

    public void Reset()
    {
        
    }
}