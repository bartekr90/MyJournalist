using MyJournalist.Domain.Common;

namespace MyJournalist.Domain.Entity;

public class Record : BaseEntity
{
    public string Name { get; set; }
    public string Content { get; set; }
    public DateTimeOffset ContentDate { get; set; }
    public bool HasContent { get; set; }
    public bool HasAnyTags { get; set; }
    public ICollection<Tag>? Tags { get; set; }
    public uint? TimeTokens { get; set; }
    public Record()
    {
        Name = "";
        Content = "";
        HasContent = false;
        HasAnyTags = false;
    }
}
