using FjernvarmeMaalingApp.Models.Interfaces;
using System.ComponentModel;

namespace FjernvarmeMaalingApp.Services.Factories.Interfaces;

public interface IConsumptionTypeFactory
{
    string Name { get; }
    IConsumptionType CreateConsumptionType();
}