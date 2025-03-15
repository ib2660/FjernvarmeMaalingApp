using FjernvarmeMaalingApp.Services.Interfaces;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class MonthlyStrategy(IWriteDataRepository dataRepository) : ITimeFrameStrategy
{
    public string Name => "Månedlig aflæsning";

    private readonly IWriteDataRepository _dataRepository = dataRepository;
    public void Execute(Measurement measurement)
    {
        if (measurement.Consumption == null || measurement.Consumption <= 0)
        {
            throw new ArgumentException("Consumption must be a positive value.");
        }
        _dataRepository.EnterData(System.Text.Json.JsonSerializer.Serialize(measurement));
    }
}
