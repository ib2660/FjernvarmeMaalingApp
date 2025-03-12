using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.ServiceInterfaces;
using System.Text.Json;

namespace FjernvarmeMaalingApp.ViewModels;

public class GemDataViewModel(ILogger<GemDataViewModel> logger, BrugerOpsætningViewModel brugerOpsætningViewModel, IWriteDataRepository writeDataRepository, JsonSerializerOptions jsonSerializerOptions)
{
    private readonly ILogger<GemDataViewModel> Logger = logger;
    private readonly IWriteDataRepository WriteDataRepository = writeDataRepository;
    private readonly JsonSerializerOptions JsonSerializerOptions = jsonSerializerOptions;
    public Measurement? Measurement { get; set; } = new();
    public string selectedTimeFrame = "Month";
    public string SelectedConsumptionTypeName { get; set; } = string.Empty;
    public string SelectedRegistrationStrategyName { get; set; } = string.Empty;
    public readonly BrugerOpsætningViewModel BrugerOpsætningViewModel = brugerOpsætningViewModel;

    public void ConsumptionTypeConfirmed()
    {
        if (SelectedConsumptionTypeName != string.Empty)
        {
            BrugerOpsætningViewModel.SetPreferredConsumptionType(BrugerOpsætningViewModel._consumptionTypeFactories[SelectedConsumptionTypeName].CreateConsumptionType());
            Measurement = new Measurement
            {
                ConsumptionType = BrugerOpsætningViewModel.PreferredConsumptionType
            };
        }
        else
        {
            Logger.LogError("No consumption type selected for instanciation of type Measurement");
        }
    }

    public void ConfirmStrategy()
    {
        if (SelectedRegistrationStrategyName != string.Empty)
        {
            var selectedStrategy = BrugerOpsætningViewModel._registrationStrategies[SelectedRegistrationStrategyName];
            Measurement!.SetConsumptionStrategy(selectedStrategy);
        }
        else
        {
            Logger.LogError("No strategy selected for Measurement instance");
        }
    }

    public void ConfirmTimeFrame()
    {
        if (Measurement != null)
        {
            Measurement.TimeFrame = selectedTimeFrame == "Month" ? TimeSpan.FromDays(30) : TimeSpan.FromDays(365);
        }
        else
        {
            Logger.LogError("Measurement is null");
        }
    }

    public void SendMeasurementData()
    {
        if (Measurement != null)
        {
            if (Measurement.Validate(out var validationResults))
            {
                var json = JsonSerializer.Serialize(Measurement);
                var success = WriteDataRepository.EnterData(json);
                if (!success)
                {
                    Logger.LogError("Failed to send measurement data");
                }
            }
            else
            {
                foreach (var validationResult in validationResults)
                {
                    Logger.LogError(validationResult.ErrorMessage);
                }
            }
        }
        else
        {
            Logger.LogError("Measurement is null");
        }
    }
}
