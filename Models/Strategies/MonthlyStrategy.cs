using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class MonthlyStrategy : ITimeFrameStrategy // TODO: Hvorfor fejler implementeringen af ITimeFrameStrategy?
{
    public string Name => "Månedlig aflæsning";
    public void Execute(Measurement measurement)
    {
        // TODO: Implementer logik for månedlig behandling
    }
}
