using FjernvarmeMaalingApp.Models.Interfaces;

namespace FjernvarmeMaalingApp.Models.Strategies;

public class MonthlyStrategy : ITimeFrameStrategy // TODO: Hvorfor fejler implementeringen af ITimeFrameStrategy?
{
    public string Name => "M�nedlig afl�sning";
    public void Execute(Measurement measurement)
    {
        // TODO: Implementer logik for m�nedlig behandling
    }
}
