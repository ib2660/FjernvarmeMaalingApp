using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class YearlyStrategy : ITimeFrameStrategy
{
    public string Name => "Årlig aflæsning";

    private readonly IWriteDataRepository _dataRepository;

    public YearlyStrategy(IWriteDataRepository dataRepository)
    {
        _dataRepository = dataRepository;
    }

    public void Execute(Measurement measurement)
    {
        if (measurement.Consumption == null || measurement.Consumption <= 0)
        {
            throw new ArgumentException("Consumption must be a positive value.");
        }

        double[] monthlyDistribution = { 0.15, 0.13, 0.13, 0.10, 0.04, 0.03, 0.02, 0.03, 0.04, 0.09, 0.11, 0.13 };
        double totalConsumption = measurement.Consumption.Value;
        List<Measurement> monthlyMeasurements = new List<Measurement>();

        for (int i = 0; i < 12; i++)
        {
            Measurement monthlyMeasurement = new Measurement
            {
                Consumption = totalConsumption * monthlyDistribution[i],
                ConsumptionType = measurement.ConsumptionType,
                RegistrationStrategy = measurement.RegistrationStrategy,
                TimeFrameStrategy = new MonthlyStrategy()
            };
            monthlyMeasurements.Add(monthlyMeasurement);
        }

        for (int i = 0; i < 11; i++)
        {
            _dataRepository.EnterData(System.Text.Json.JsonSerializer.Serialize(monthlyMeasurements[i]));
        }

        // Adjust the original measurement to contain only the last month's data
        measurement.Consumption = totalConsumption * monthlyDistribution[11];
        measurement.TimeFrameStrategy = new MonthlyStrategy();
    }
}
