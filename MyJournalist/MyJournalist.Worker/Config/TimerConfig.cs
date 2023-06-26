using System.ComponentModel.DataAnnotations;

namespace MyJournalist.Worker.Config;

public class TimerConfig : ITimerConfig
{    
    [Required]
    [TimeSpanRange("00:00:00", "23:59:59", ErrorMessage = "MeasurementTime must be between 00:00:00 and 23:59:59")]
    public TimeSpan MeasurementTime { get; set; }    

    [Required]
    [Range(0, int.MaxValue)]
    public int PeriodInHours { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int PeriodInMinutes { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int PeriodInSeconds { get; set; }
}
