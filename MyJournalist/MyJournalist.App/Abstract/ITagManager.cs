using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Abstract;

public interface ITagManager
{
    List<Tag> GetTagsFromContent(string contentFromTxt);
    void LoadTagsFromFile();
    void MergedTagsSave(ICollection<Tag> tags);
    List<Tag> MergeTags(ICollection<Tag>? primaryTags, ICollection<Tag>? newTags);
    List<Tag> MergeTagsFromRecords(List<Record> records);
}
