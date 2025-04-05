using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Strategies.StrategyComponents;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.Metrics;
using System.Globalization;
namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class SameMonthAllYearsDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis samme måned i alle år";
    private IEnumerable<Measurement> _filteredMeasurements = [];
    private IEnumerable<Measurement> _measurements = [];
    private EventCallback<IEnumerable<Measurement>> _eventCallback;

    public RenderFragment Execute(IEnumerable<Measurement> measurements, EventCallback<IEnumerable<Measurement>> eventCallback)
    {
        _eventCallback = eventCallback;
        _measurements = measurements;        
        return BuildRenderFragment();
    }

    private RenderFragment BuildRenderFragment()
    {
        return builder =>
        {
            builder.OpenElement(0, "h3");
            builder.AddContent(1, "Vis målinger fra samme måned i alle år");
            builder.CloseElement();

            builder.OpenElement(2, "div");
            builder.AddContent(3, "Vælg måned: ");
            builder.OpenElement(4, "input");
            builder.AddAttribute(5, "id", "monthChosen");
            builder.AddAttribute(6, "type", "month");
            builder.AddAttribute(7, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, OnMonthChanged));
            builder.CloseElement();
            builder.CloseElement();

            builder.OpenComponent<DisplayComponent>(8);
            builder.AddAttribute(9, "Measurements", _filteredMeasurements);
            builder.CloseComponent();
        };
    }

    private void OnMonthChanged(ChangeEventArgs e)
    {
        if (DateOnly.TryParse(e.Value?.ToString(), out var month))
        {
            FilterMeasurements(month);
            _eventCallback.InvokeAsync(_filteredMeasurements);
        }
    }

    private void FilterMeasurements(DateOnly month)
    {
        _filteredMeasurements = _measurements.Where(m =>
            m.MeasurementDate.HasValue &&
            m.MeasurementDate.Value.Month == month.Month).ToList();
    }

    public void Reset()
    {
        _filteredMeasurements = [];
    }
}
