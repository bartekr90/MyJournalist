namespace MyJournalist.App.Abstract;

public interface IRecordService : IEntityService<Record>
{
    Record GetRecordFromContent(string contentFromTxt, ICollection<Tag> tags, IDateTimeProvider provider);
}
