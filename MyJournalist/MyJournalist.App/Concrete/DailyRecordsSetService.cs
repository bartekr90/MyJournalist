using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.Domain.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MyJournalist.App.Concrete;

public class DailyRecordsSetService : BaseEntityService<DailyRecordsSet>, IEqualityComparer<DailyRecordsSet>, IDailyRecordsSetService
{
    public DateTimeOffset RefersToDate { get; set; }

    public override string GetFileName()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(RefersToDate.ToString("MMMM"));
        sb.Append('_');
        sb.Append(RefersToDate.Year.ToString());

        return @$"RecordBook_" + sb.ToString();
    }

    public override DailyRecordsSet GetEmptyObj(IDateTimeProvider provider)
    {
        return new DailyRecordsSet
        {
            HasAnyRecords = false,
            CreatedDateTime = provider.Now,
            ModifiedDateTime = provider.Now,
            RefersToDate = RefersToDateVerification(provider),
            EmailHasBeenSent = false,
            HasAnyTags = false,
            MergedContents = "no content",
        };
    }

    private DateTimeOffset RefersToDateVerification(IDateTimeProvider provider)
    {
        if (RefersToDate == DateTimeOffset.MinValue)
        {
            var date = RefersToDate.Date.ToDateTimeOffset();
            return date <= provider.Now ? date : provider.Now;
        }
        return RefersToDate <= provider.Now ? RefersToDate : provider.Now;
    }

    private string MergeContent(ICollection<Record> records)
    {
        StringBuilder mergedContent = new StringBuilder();

        foreach (Record record in records)
        {
            if (record.HasContent)
            {
                mergedContent.AppendLine(record.Content);
                mergedContent.AppendLine();
                mergedContent.AppendLine("***");
                mergedContent.AppendLine();
            }
        }
        return mergedContent.ToString();
    }

    public DailyRecordsSet GetDailyRecordsSet(ICollection<Record> records, ICollection<Tag>? tags, IDateTimeProvider provider)
    {
        var content = MergeContent(records);

        if (string.IsNullOrEmpty(content))
            content = "no content";
        return new DailyRecordsSet
        {
            Records = records,
            HasAnyRecords = true,
            CreatedDateTime = provider.Now,
            ModifiedDateTime = provider.Now,
            RefersToDate = RefersToDateVerification(provider),
            EmailHasBeenSent = false,
            MergedTags = tags,
            HasAnyTags = (tags == null || tags.Count == 0) ? false : true,
            MergedContents = content
        };
    }

    public List<DailyRecordsSet> MakeUnion(ICollection<DailyRecordsSet>? primarySets, ICollection<DailyRecordsSet>? newSets)
    {
        if (primarySets == null) return newSets?.ToList();
        return newSets == null ? primarySets.ToList() : primarySets.Union(newSets, this).ToList();
    }

    private void MergeDailySets(DailyRecordsSet existingDailySets, DailyRecordsSet dailySet)
    {
        List<Record> recordList = existingDailySets.Records as List<Record> ?? new List<Record>();
        recordList.AddRange(dailySet.Records ?? new List<Record>());

        List<Tag> tags = existingDailySets.MergedTags as List<Tag> ?? new List<Tag>();
        tags.AddRange(dailySet.MergedTags ?? new List<Tag>());

        existingDailySets.Records = recordList;
        existingDailySets.MergedTags = tags;
        existingDailySets.MergedContents += dailySet.MergedContents;
        existingDailySets.EmailHasBeenSent |= dailySet.EmailHasBeenSent;
        existingDailySets.HasAnyRecords |= dailySet.HasAnyRecords;
        existingDailySets.HasAnyTags |= dailySet.HasAnyTags;
    }

    public bool Equals(DailyRecordsSet set1, DailyRecordsSet set2)
    {
        var areEqual = DateTimeOffset.Equals(set1.RefersToDate.Date, set2.RefersToDate.Date);

        if (areEqual)
            MergeDailySets(set1, set2);

        return areEqual;
    }

    public int GetHashCode([DisallowNull] DailyRecordsSet obj) =>
         obj.RefersToDate.Date.ToString().ToLower().GetHashCode();

}
