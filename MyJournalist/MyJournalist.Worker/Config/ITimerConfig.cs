namespace MyJournalist.Worker.Config;

public interface ITimerConfig
{
    TimeSpan MeasurementTime { get; set; }
    int PeriodInHours { get; set; }
    int PeriodInMinutes { get; set; }
    int PeriodInSeconds { get; set; }
}