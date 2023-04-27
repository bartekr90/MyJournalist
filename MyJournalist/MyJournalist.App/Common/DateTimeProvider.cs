using MyJournalist.App.Abstract;

namespace MyJournalist.App.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now;

    public DateTimeOffset UtcTime => DateTimeOffset.UtcNow;

    public DateTimeOffset DateOfContent { get; set; }
}
