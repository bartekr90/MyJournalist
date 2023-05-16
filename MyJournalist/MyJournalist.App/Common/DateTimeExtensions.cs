namespace MyJournalist.App.Common;

public static class DateTimeExtensions
{
    public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
    {
        return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                   ? DateTimeOffset.MinValue
                   : new DateTimeOffset(dateTime);
    }
}