﻿using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Factories;

public class OlieForbrugFactory : IConsumptionTypeFactory
{
    public string Name { get; private set; } = OlieForbrugLiter.Instance.ConsumptionTypeName;
    public IConsumptionType CreateConsumptionType()
    {
        return OlieForbrugLiter.Instance;
    }
}
