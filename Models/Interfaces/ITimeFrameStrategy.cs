using FjernvarmeMaalingApp.Models;

public interface ITimeFrameStrategy
{
    string Name { get; }
    void Execute(Measurement measurement);
}
