using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Strategies.StrategyComponents;
using Microsoft.AspNetCore.Components;

namespace FjernvarmeMaalingApp.ViewModels.Strategies;
public class ListMonthsDisplayStrategy : IMeasurementDisplayStrategy
{
    public string DisplayName { get; } = "Vis liste af måneder";
    private IEnumerable<Measurement> _filteredMeasurements = [];
    private IEnumerable<Measurement> _measurements = [];
    private EventCallback<IEnumerable<Measurement>> _eventCallback;
    private DateOnly _firstMonth;
    private bool _firstSet;
    private DateOnly _lastMonth;
    private bool _lastSet;
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
            builder.AddContent(1, "Vis målinger for en række af måneder");
            builder.CloseElement();

            builder.OpenElement(2, "div");
            builder.AddContent(3, "Vælg første måned: ");
            builder.OpenElement(4, "input");
            builder.AddAttribute(5, "id", "firstMonthChosen");
            builder.AddAttribute(6, "type", "month");
            builder.AddAttribute(7, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnMonthChanged(e, "firstMonthChosen")));
            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(8, "div");
            builder.AddContent(9, "Vælg sidste måned: ");
            builder.OpenElement(10, "input");
            builder.AddAttribute(11, "id", "lastMonthChosen");
            builder.AddAttribute(12, "type", "month");
            builder.AddAttribute(13, "onchange", EventCallback.Factory.Create<ChangeEventArgs>(this, e => OnMonthChanged(e, "lastMonthChosen")));
            builder.CloseElement();
            builder.CloseElement();

            builder.OpenComponent<DisplayComponent>(14);
            builder.AddAttribute(15, "Measurements", _filteredMeasurements);
            builder.CloseComponent();
        };
    }

    private void OnMonthChanged(ChangeEventArgs e, string id)
    {
        if (id == "firstMonthChosen")
        {
            _firstSet = DateOnly.TryParse(e.Value?.ToString(), out _firstMonth);
        }
        else _lastSet = DateOnly.TryParse(e.Value?.ToString(), out _lastMonth);
        if (_firstSet && _lastSet )
        { 
            FilterMeasurements();
            _eventCallback.InvokeAsync(_filteredMeasurements);
        }
    }

    private void FilterMeasurements()
    {
        _filteredMeasurements = _measurements.Where(m =>
            m.MeasurementDate.HasValue &&
            m.MeasurementDate.Value >= _firstMonth &&
            m.MeasurementDate.Value <= _lastMonth)
            .ToList();
    }

    public void Reset()
    {
    }
}
