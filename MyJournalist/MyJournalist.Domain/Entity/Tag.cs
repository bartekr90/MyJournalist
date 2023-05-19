using MyJournalist.Domain.Common;

namespace MyJournalist.Domain.Entity;

public class Tag : BaseEntity
{
    public string Name { get; set; }
    public int IdOfInitialRecord { get; set; }
    public DateTimeOffset ContentDate { get; set; }
    public string NameOfInitialRecord { get; set; }
    public uint TimeTokens { get; set; }
    public ICollection<Record>? Records { get; set; }

    public Tag()
    {
        Name = "";
        IdOfInitialRecord = 0;
        NameOfInitialRecord = string.Empty;
    }
}
