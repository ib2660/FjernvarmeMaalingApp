using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class YearlyTimeFrameStrategy(IWriteDataRepository dataRepository) : ITimeFrameStrategy
{
    public string Name => "�rlig afl�sning";

    private readonly IWriteDataRepository _dataRepository = dataRepository;

    public void Execute(Measurement measurement)
    {
        if (measurement.Consumption == null || measurement.Consumption <= 0)
        {
            throw new ArgumentException("Consumption must be a positive value.");
        }

        double[] monthlyDistribution = { 0.13, 0.11, 0.09, 0.04, 0.03, 0.02, 0.03, 0.04, 0.10, 0.13, 0.13, 0.15 }; // omvendt r�kkef�lge dec => jan
        double totalConsumption = measurement.Consumption.Value;
        for (int i = 0; i < 12; i++)
        {
            Measurement monthlyMeasurement = new()
            {
                MeasurementDate = measurement.MeasurementDate!.Value.AddMonths(-1 * i),
                Consumption = totalConsumption * monthlyDistribution[i],
                RegisteredBy = measurement.RegisteredBy,
                TimeFrame = "M�nedlig afl�sning",
                ConsumptionType = measurement.ConsumptionType,
            };
            _dataRepository.EnterData(System.Text.Json.JsonSerializer.Serialize(monthlyMeasurement));
        }
    }
}
