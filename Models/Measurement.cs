using FjernvarmeMaalingApp.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace FjernvarmeMaalingApp.Models;

public class Measurement : IComparable<Measurement>
{
    [Required(ErrorMessage = "Dato skal udfyldes.")] public DateOnly? MeasurementDate { get; set; }
    [Range(0, double.MaxValue, ErrorMessage = "Forbrug skal være en positiv værdi.")] public double? Consumption { get; set; }
    [Required] public string RegisteredBy { get; set; }
    [Required] public string TimeFrame { get; set; }
    [Required] public string ConsumptionType { get; set; }

    public int CompareTo(Measurement? other)
    {
        if (other == null || other.MeasurementDate == null)
            return 1;
        if (ReferenceEquals(this, other))
            return 0;
        if (MeasurementDate != null && other.MeasurementDate != null)
        {
            return MeasurementDate.Value.CompareTo(other.MeasurementDate.Value);
        }
        return -1;
    }

    public bool Validate(out List<ValidationResult> validationResults)
    {
        ValidationContext validationContext = new(this);
        validationResults = [];
        return Validator.TryValidateObject(this, validationContext, validationResults, true);
    }
}