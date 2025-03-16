using FjernvarmeMaalingApp.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FjernvarmeMaalingApp.Models;

public class Measurement
{
    [Required(ErrorMessage = "Dato skal udfyldes.")] public DateOnly? MeasurementDate { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Forbrug skal være en positiv værdi.")] public double? Consumption { get; set; }
    [Required] public string RegisteredBy { get; set; }
    [Required] public string TimeFrame { get; set; }
    [Required] public string ConsumptionType { get; set; }
    public bool Validate(out List<ValidationResult> validationResults)
    {
        ValidationContext validationContext = new(this);
        validationResults = [];
        return Validator.TryValidateObject(this, validationContext, validationResults, true);
    }
}