using System.ComponentModel.DataAnnotations;
using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models;

public class Measurement
{
    // TODO: inkludere validation attributter på nogle af felterne.
    // TODO: inkludere en liste af fejlbeskeder, som kan vises til brugeren.
    // TODO: Det skal også lægges ud i viewmodellen, så det kan vises i brugergrænsefladen.

    [Required(ErrorMessage = "TimeFrame is required.")]
    public TimeSpan? TimeFrame { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Consumption must be a non-negative value.")]
    public double? Consumption { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "StartMeasure must be a non-negative value.")]
    public double? StartMeasure { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "EndMeasure must be a non-negative value.")]
    public double? EndMeasure { get; set; }

    public IConsumptionType? ConsumptionType { get; set; }
    public IRegistrationStrategy? RegistrationStrategy { get; set; }

    public void SetConsumptionStrategy(IRegistrationStrategy strategy)
    {
        RegistrationStrategy = strategy;
    }

    public void ExecuteConsumptionStrategy()
    {
        if (RegistrationStrategy == null)
        {
            return;
        }
        RegistrationStrategy.Execute(this);
    }

    public bool Validate(out List<ValidationResult> validationResults)
    {
        var validationContext = new ValidationContext(this);
        validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(this, validationContext, validationResults, true);
    }
}
