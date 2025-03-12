using FjernvarmeMaalingApp.Models.Interfaces;
using System.ComponentModel;

namespace FjernvarmeMaalingApp.Services.Factories.Interfaces;

public interface IConsumptionTypeFactory
{
    string ConsumptionTypeName { get; }
    IConsumptionType CreateConsumptionType();
}