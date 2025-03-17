using FjernvarmeMaalingApp.Models;

public interface ITimeFrameStrategy
{
    string Name { get; }
    List<Measurement> Execute(Measurement measurement);
}
