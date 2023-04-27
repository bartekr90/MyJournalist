using MyJournalist.Domain.Common;

namespace MyJournalist.Domain.Entity;

public class DailyRecordsSet : BaseEntity
{
    public ICollection<Record>? Records { get; set; }
    public DateTimeOffset RefersToDate { get; set; }
    public ICollection<Tag>? MergedTags { get; set; }
    public string MergedContents { get; set; }
    public bool EmailHasBeenSent { get; set; }
    public bool HasAnyRecords { get; set; }
    public bool HasAnyTags { get; set; }
    public DailyRecordsSet()
    {
        EmailHasBeenSent = false;
        HasAnyRecords = false;
        MergedContents = string.Empty;
    }
}
