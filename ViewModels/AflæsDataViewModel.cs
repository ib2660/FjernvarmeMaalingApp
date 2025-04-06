using CommunityToolkit.Mvvm.ComponentModel;
using FjernvarmeMaalingApp.Data;
using FjernvarmeMaalingApp.Data.Interfaces;
using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Services;
using FjernvarmeMaalingApp.Services.Interfaces;
using FjernvarmeMaalingApp.ViewModels.Interfaces;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace FjernvarmeMaalingApp.ViewModels;

public partial class AflæsDataViewModel(IAuthenticationService authenticationService, IUserRepository userRepository, IReadDataRepository readDataRepository, ILogger<AflæsDataViewModel> logger, IServicesRegistry servicesRegistry) : ObservableObject
{
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IReadDataRepository _readDataRepository = readDataRepository;
    private readonly ILogger<AflæsDataViewModel> _logger = logger;
    private readonly IServicesRegistry _servicesRegistry = servicesRegistry;
    private List<Measurement> measurements = [];
    private IMeasurementDisplayStrategy? _currentMeasurementDisplayStrategy;

    [ObservableProperty]
    private string selectedMeasurementDisplayStrategyName = string.Empty;

    partial void OnSelectedMeasurementDisplayStrategyNameChanged(string value)
    {
        ResetCurrentStrategy();
    }

    public User? CurrentUser { get; private set; }
    public async Task InitializeAsync()
    {
        var claimsPrincipal = await _authenticationService.GetCurrentUserAsync();
        var username = claimsPrincipal.Identity?.Name;
        if (username != null && username != CurrentUser?.Username)
        {
            CurrentUser = (User?) await _userRepository.GetUserAsync(username);
        }
        if (CurrentUser == null || username == null)
        {
            _logger.LogError("User not logged in.");
            return;
        }
        if (measurements.Count == 0)
        {
            measurements = await GetMeasurementsAsync();
        }
    }
    public List<string> GetMeasurementsDisplayStrategyNames()
    {
        return [.. _servicesRegistry.GetAllMeasurementDisplayStrategies().Select(m => m.DisplayName)];
    }

    public async Task<List<Measurement>> GetMeasurementsAsync()
    {
        Measurement temp = null;
        List<Measurement> result = [];
        if (CurrentUser != null)
        {
            string json = await _readDataRepository.ReadData(CurrentUser);
            if (json != string.Empty)
            {
                List<string> strings = ReadDataRepository.SplitJsonObjects(json);
                foreach (string s in strings)
                {
                    temp = JsonHelper.DeserializeObject<Measurement>(s);
                    if (temp != null && temp.Consumption != null)
                    {
                        temp.Consumption = GetConvertedConsumption(temp.Consumption.Value, temp.ConsumptionType);
                        result.Add(temp);
                    }
                }
                result.Sort();
            }
        }
        return result;
    }
    public double GetConvertedConsumption(double consumption, string strategyName)
    {
        double result = 0;
        var strategy = _servicesRegistry.GetConsumptionType(strategyName);
        if (strategy != null)
        {
            result = strategy.EnergyEquivalent * consumption;
        }
        return result;
    }
    public RenderFragment? ExecuteDisplayStrategy(EventCallback<IEnumerable<Measurement>> eventCallback)
    {
        if (SelectedMeasurementDisplayStrategyName == null || measurements.Count == 0) return null;
        var strategy = _servicesRegistry.GetMeasurementDisplayStrategy(SelectedMeasurementDisplayStrategyName);
        if (strategy == null) return null;
        _currentMeasurementDisplayStrategy = strategy;
        return strategy!.Execute(measurements, eventCallback);
    }

    private void ResetCurrentStrategy()
    {
        if (_currentMeasurementDisplayStrategy == null) return;
        _currentMeasurementDisplayStrategy.Reset();
    }
}


