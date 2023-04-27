using System.Diagnostics.CodeAnalysis;

namespace MyJournalist.App.Abstract;

public interface IDailyRecordsSetService : IEntityService<DailyRecordsSet>
{
    DateTimeOffset RefersToDate { get; set; }
    bool Equals(DailyRecordsSet set1, DailyRecordsSet set2);
    DailyRecordsSet GetDailyRecordsSet(ICollection<Record> records, ICollection<Tag>? tags, IDateTimeProvider dateTimeProvider);
    int GetHashCode([DisallowNull] DailyRecordsSet obj);
    List<DailyRecordsSet> MakeUnion(ICollection<DailyRecordsSet>? primarySets, ICollection<DailyRecordsSet>? newSets);
}
