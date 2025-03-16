﻿using FjernvarmeMaalingApp.Models;
using FjernvarmeMaalingApp.Models.Interfaces;
using FjernvarmeMaalingApp.Services.Factories.Interfaces;

namespace FjernvarmeMaalingApp.Services.Factories;

public class FjernvarmeForbrugFactory : IConsumptionTypeFactory
{
    public string Name { get; private set;  }= "Fjernvarmeforbrug i MWh";
    public IConsumptionType CreateConsumptionType()
    {
        return FjernvarmeForbrugMWh.Instance;
    }
}
