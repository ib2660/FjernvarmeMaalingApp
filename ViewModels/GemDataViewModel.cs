using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services.Interfaces;
using System.Text.Json;

namespace FjernvarmeMaalingApp.ViewModels;

public class GemDataViewModel(ILogger<GemDataViewModel> logger, BrugerOpsætningViewModel brugerOpsætningViewModel, IWriteDataRepository writeDataRepository, JsonSerializerOptions jsonSerializerOptions)
{
    private readonly ILogger<GemDataViewModel> _logger = logger;
    private readonly IWriteDataRepository _writeDataRepository = writeDataRepository;
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
    public readonly BrugerOpsætningViewModel BrugerOpsætningViewModel = brugerOpsætningViewModel;
    public Measurement? Measurement { get; set; }
    public Action? OnStateChange { get; set; }
    public string selectedTimeFrame = "Månedlig aflæsning";
    public string SelectedConsumptionTypeName { get; set; } = string.Empty;
    public string SelectedRegistrationStrategyName { get; set; } = string.Empty;
    
    public async Task ConsumptionTypeConfirmedAsync()
    {
        if (SelectedConsumptionTypeName != string.Empty)
        {
            BrugerOpsætningViewModel.SelectedConsumptionType = SelectedConsumptionTypeName;
            await BrugerOpsætningViewModel.UpdateUserDetails();
            Measurement = new Measurement
            {
                ConsumptionType = BrugerOpsætningViewModel.PreferredConsumptionType
            };
            // OnStateChange?.Invoke();
        }
        else
        {
            _logger.LogError("No consumption type selected for instanciation of type Measurement");
        }
    }

    public void ConfirmRegistrationStrategy()
    {
        if (SelectedRegistrationStrategyName != string.Empty)
        {
            var selectedStrategy = BrugerOpsætningViewModel.RegistrationStrategies[SelectedRegistrationStrategyName];
            Measurement!.SetRegistrationStrategy(selectedStrategy);
            OnStateChange?.Invoke();
        }
        else
        {
            _logger.LogError("No strategy selected for Measurement instance");
        }
    }

    public void ConfirmTimeFrameStrategy()
    {
        if (Measurement != null)
        {
            Measurement.SetTimeFrameStrategy(selectedTimeFrame);
            OnStateChange?.Invoke();
        }
        else
        {
            _logger.LogError("Measurement is null");
        }
    }
    public void ConfirmMeasurementDate()
    {
        if (Measurement != null)
        {
            if (Measurement.MeasurementDate == null)
            {
                _logger.LogError("Measurement date is null");
            }
            _logger.LogInformation($"Measurement date set to {Measurement.MeasurementDate}");
            OnStateChange?.Invoke();
        }
        else
        {
            _logger.LogError("Measurement is null");
        }
    }

    public void SendMeasurementData()
    {
        if (Measurement != null)
        {
            Measurement.ExecuteRegistrationStrategy();
            if (Measurement.Validate(out var validationResults))
            {
                var json = JsonSerializer.Serialize(Measurement);
                var success = _writeDataRepository.EnterData(json);
                if (!success)
                {
                    _logger.LogError("Failed to send measurement data");
                }
            }
            else
            {
                foreach (var validationResult in validationResults)
                {
                    _logger.LogError(validationResult.ErrorMessage);
                }
            }
        }
        else
        {
            _logger.LogError("Measurement is null");
        }
        Measurement = new Measurement();
        OnStateChange?.Invoke();
    }
}
