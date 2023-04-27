using MyJournalist.Domain.Entity;
using System.Diagnostics.CodeAnalysis;

namespace MyJournalist.App.Abstract;

public interface ITagService : IEntityService<Tag>
{
    bool Equals(Tag tag1, Tag tag2);
    List<Tag> FindAllTags(string content, IDateTimeProvider provider, uint tokens);
    uint FindTokens(string content);
    int GetHashCode([DisallowNull] Tag obj);
    Tag GetTag(string name, IDateTimeProvider provider, uint tokens);
    List<Tag> MakeUnion(ICollection<Tag>? primaryTags, ICollection<Tag>? newTags);
}
