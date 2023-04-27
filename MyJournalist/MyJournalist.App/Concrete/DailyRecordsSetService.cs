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
        var dailySet = new DailyRecordsSet
        {
            HasAnyRecords = false,
            CreatedDateTime = provider.Now,
            ModifiedDateTime = provider.Now,
            RefersToDate = RefersToDate.Date,
            EmailHasBeenSent = false,
            HasAnyTags = false,
            MergedContents = "no content have been recorded",
        };
        return dailySet;
    }

    private string MergeContent(ICollection<Record> records)
    {
        StringBuilder mergedContent = new StringBuilder();

        foreach (Record record in records)
        {
            if (record.HasContent)
            {
                mergedContent.Append(record.Content);
                mergedContent.Append("\n***\n");
            }
        }
        if (mergedContent.Length >= 6)
            mergedContent.Remove(mergedContent.Length - 6, 6);

        return mergedContent.ToString();
    }

    public DailyRecordsSet GetDailyRecordsSet(ICollection<Record> records, ICollection<Tag>? tags, IDateTimeProvider dateTimeProvider)
    {
        string content = MergeContent(records);
        var dailySet = new DailyRecordsSet
        {
            Records = records,
            HasAnyRecords = true,
            CreatedDateTime = dateTimeProvider.Now,
            ModifiedDateTime = dateTimeProvider.Now,
            RefersToDate = RefersToDate.Date,
            EmailHasBeenSent = false,
            MergedTags = tags,
            HasAnyTags = (tags == null || tags.Count == 0) ? false : true,
            MergedContents = content,
        };
        return dailySet;
    }

    public List<DailyRecordsSet> MakeUnion(ICollection<DailyRecordsSet>? primarySets, ICollection<DailyRecordsSet>? newSets)
    {
        if (primarySets == null && newSets == null)
            return null;
        else if (primarySets == null)
            return newSets.ToList();
        else if (newSets == null)
            return primarySets.ToList();

        var mergedSets = primarySets.Union(newSets, this);

        return mergedSets.ToList();
    }

    private void MergeDailySets(DailyRecordsSet existingDailySets, DailyRecordsSet dailySet)
    {
        List<Record> recordList = existingDailySets.Records as List<Record> ?? new List<Record>();
        recordList.AddRange(dailySet.Records ?? new List<Record>());

        List<Tag> tags = existingDailySets.MergedTags as List<Tag> ?? new List<Tag>();
        tags.AddRange(dailySet.MergedTags ?? new List<Tag>());

        existingDailySets.Records = recordList;
        existingDailySets.MergedTags = tags;
        existingDailySets.MergedContents += ("\n***\n");
        existingDailySets.MergedContents += dailySet.MergedContents;
        existingDailySets.EmailHasBeenSent |= dailySet.EmailHasBeenSent;
        existingDailySets.HasAnyRecords |= dailySet.HasAnyRecords;
        existingDailySets.HasAnyTags |= dailySet.HasAnyTags;
    }

    public bool Equals(DailyRecordsSet set1, DailyRecordsSet set2)
    {
        var areEqual = DateTimeOffset.Equals(set1.RefersToDate, set2.RefersToDate);

        if (areEqual)
            MergeDailySets(set1, set2);

        return areEqual;
    }

    public int GetHashCode([DisallowNull] DailyRecordsSet obj)
    {
        return obj.RefersToDate.ToString().ToLower().GetHashCode();

    }
}
