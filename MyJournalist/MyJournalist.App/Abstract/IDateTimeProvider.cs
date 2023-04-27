namespace MyJournalist.App.Abstract;

public interface IDateTimeProvider
{
    public DateTimeOffset DateOfContent { get; set; }
    public DateTimeOffset Now { get; }
    public DateTimeOffset UtcTime { get; }
}
