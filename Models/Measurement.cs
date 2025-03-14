using System.ComponentModel.DataAnnotations;
using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models;

public class Measurement
{
    // TODO: Der skal lægges en aflæsningsdato ind i modellen.
    // TODO: Dato skal skrives med i datarepository.
    [Required(ErrorMessage = "Dato skal udfyldes.")]
    public DateOnly? MeasurementDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Forbrug skal være en positiv værdi.")]
    public double? Consumption { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Startmåling skal være en positiv værdi.")]
    public double? StartMeasure { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Slutmåling skal være en positiv værdi.")]
    public double? EndMeasure { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Det beregnede forbrug skal være en positiv værdi.")]
    public double? ConvertedConsumption { get; private set; }
    public IConsumptionType? ConsumptionType { get; set; }
    public IRegistrationStrategy? RegistrationStrategy { get; set; }
    public ITimeFrameStrategy? TimeFrameStrategy { get; set; }
    private IEnumerable<ITimeFrameStrategy> _timeFrameStrategies;
    public Measurement()
    {
    }
    public Measurement(IEnumerable<ITimeFrameStrategy> timeFrameStrategies)
    {
        _timeFrameStrategies = timeFrameStrategies;
    }
    public void SetRegistrationStrategy(IRegistrationStrategy strategy)
    {
        RegistrationStrategy = strategy;
    }

    public void ExecuteRegistrationStrategy()
    {
        if (RegistrationStrategy == null)
        {
            return;
        }
        RegistrationStrategy.Execute(this);
        ConvertedConsumption = Consumption / ConsumptionType!.EnergyEquivalent;
    }

    public void SetTimeFrameStrategy(string timeFramStrategyName)
    {
        TimeFrameStrategy = _timeFrameStrategies.FirstOrDefault(s => s.Name == timeFramStrategyName);
    }

    public void ExecuteTimeFrameStrategy()
    {
        if (TimeFrameStrategy == null)
        {
            return;
        }
        TimeFrameStrategy.Execute(this);
    }

    public bool Validate(out List<ValidationResult> validationResults)
    {
        var validationContext = new ValidationContext(this);
        validationResults = [];
        return Validator.TryValidateObject(this, validationContext, validationResults, true);
    }
}
