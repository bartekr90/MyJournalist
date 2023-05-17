using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.Domain.Entity;
using System.Text;

namespace MyJournalist.App.Concrete;

public class RecordService : BaseEntityService<Record>, IRecordService
{

    public Record GetRecordFromContent(string contentFromTxt, ICollection<Tag> tags, IDateTimeProvider provider)
    {
        string nameOfRecord = GetNameOfRecord(provider.DateOfContent);
        uint tokens = 0;

        bool hasAnyTags = (tags == null || tags.Count == 0) ? false : true;

        if (hasAnyTags)
        {
            tokens = tags.First().TimeTokens;
            foreach (var tag in tags)
            {
                tag.NameOfInitialRecord = nameOfRecord;
            }
        }

        return new Record
        {
            Name = nameOfRecord,
            Content = contentFromTxt,
            ContentDate = provider.DateOfContent,
            HasContent = !string.IsNullOrWhiteSpace(contentFromTxt),
            HasAnyTags = hasAnyTags,
            Tags = tags,
            TimeTokens = tokens,
            CreatedDateTime = provider.Now,
            ModifiedDateTime = provider.Now,
        };               
    }

    private string GetNameOfRecord(DateTimeOffset contentDate)
    {
        var data = contentDate.ToString().ToCharArray();

        StringBuilder sb = new StringBuilder();
        sb.Append(new char[] { 'R', 'e', 'c', '_' });
        sb.Append(data, 0, 10);
        sb.Append(data, 10, 6);

        sb.Replace('.', '/');
        sb.Replace(' ', '_');
        sb.Replace(':', '-');

        return sb.ToString();
    }

    public override Record GetEmptyObj(IDateTimeProvider provider)
    {
        return new Record
        {
            Id = 0,
            Name = GetNameOfRecord(provider.DateOfContent),
            ContentDate = provider.DateOfContent,
            Content = "[no content]",
            HasAnyTags = false,
            HasContent = false,
            CreatedDateTime = provider.Now,
            ModifiedDateTime = provider.Now
        };
    }

    public override string GetFileName()
    {
        return @"Records_from_last_24_h";
    }

}

