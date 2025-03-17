using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;
using System.Text;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class YearlyTimeFrameStrategy(ILogger<YearlyTimeFrameStrategy> logger) : ITimeFrameStrategy
{
    private readonly ILogger<YearlyTimeFrameStrategy> _logger = logger;
    public string Name => "Årlig aflæsning";

    public List<Measurement> Execute(Measurement measurement)
    {
        if (measurement.Consumption == null || measurement.Consumption <= 0)
        {
            throw new ArgumentException("Consumption must be a positive value.");
        }

        double[] monthlyDistribution = { 0.13, 0.11, 0.09, 0.04, 0.03, 0.02, 0.03, 0.04, 0.10, 0.13, 0.13, 0.15 }; // omvendt rækkefølge dec => jan
        double totalConsumption = measurement.Consumption.Value;
        List<Measurement> measurements = [];
        for (int i = 0; i < 12; i++)
        {
            Measurement m = new()
            {
                MeasurementDate = measurement.MeasurementDate!.Value.AddMonths(-1 * i),
                Consumption = totalConsumption * monthlyDistribution[i],
                RegisteredBy = measurement.RegisteredBy,
                TimeFrame = "Månedlig aflæsning",
                ConsumptionType = measurement.ConsumptionType,
            };
            m.Validate(out var validationResults);
            if (validationResults.Count > 0)
            {
                foreach (var validationResult in validationResults)
                {
                    _logger.LogError(validationResult.ErrorMessage);
                }
            }
            else
            {
                measurements.Add(m);
            }
        }
        return measurements;
    }
}
