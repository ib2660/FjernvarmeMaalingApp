using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class MonthlyTimeFrameStrategy(ILogger<MonthlyTimeFrameStrategy> logger) : ITimeFrameStrategy
{
    private readonly ILogger<MonthlyTimeFrameStrategy> _logger = logger;
    public string Name => "Månedlig aflæsning";
    public List<Measurement> Execute(Measurement measurement)
    {
            if (measurement.Consumption == null || measurement.Consumption <= 0)
            {
                throw new ArgumentException("Consumption must be a positive value.");
            }
        measurement.TimeFrame = Name;
        measurement.Validate(out var validationResults);
        if (validationResults.Count > 0)
        {
            foreach (var validationResult in validationResults)
            {
                _logger.LogError(validationResult.ErrorMessage);
            }
            return [];
        }
        return [measurement];
    }
}
