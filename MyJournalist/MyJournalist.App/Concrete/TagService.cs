using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.Domain.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace MyJournalist.App.Concrete;

public class TagService : BaseEntityService<Tag>, IEqualityComparer<Tag>, ITagService
{
    private static readonly Regex _tagRegex =
       new(@"\#\w+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(500));
    private static readonly Regex _tokenregex =
        new(@"\$[0-9]+", RegexOptions.Compiled, TimeSpan.FromMilliseconds(500));

    public override string GetFileName()
    {
        return @"Tags_list";
    }

    public override Tag GetEmptyObj(IDateTimeProvider provider)
    {
        return new Tag
        {
            ModifiedDateTime = provider.Now,
            CreatedDateTime = provider.Now,
            ContentDate = provider.DateOfContent,
        };
    }

    public List<Tag> MakeUnion(ICollection<Tag>? primaryTags, ICollection<Tag>? newTags)
    {
        if (primaryTags == null && newTags == null)
            return null!;
        else if (primaryTags == null || primaryTags.Count == 0)
            return newTags!.ToList();
        else if (newTags == null || newTags.Count == 0)
            return primaryTags.ToList();

        return primaryTags.Union(newTags, this).ToList();
    }

    public uint FindTokens(string content)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(content))
                return 0;

            Match match = _tokenregex.Match(content);

            if (match.Success)
            {
                string tokens = match.Value.Substring(1);

                if (uint.TryParse(tokens, out uint result))
                    return result;
            }

            return 0;
        }
        catch (RegexMatchTimeoutException)
        {
            return 0;
        }
    }

    public Tag GetTag(string tagName, IDateTimeProvider provider, uint tokens)
    {
        if (string.IsNullOrWhiteSpace(tagName))
            tagName = "DefaultName";

        Tag tag = GetEmptyObj(provider);
        tag.Name = tagName;
        tag.TimeTokens = tokens;
        return tag;
    }

    public List<Tag> FindAllTags(string content, IDateTimeProvider provider, uint tokens)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            var tagMatches = _tagRegex.Matches(content);

            if (!tagMatches.Any())
                return null;

            var tags = new List<Tag>();
            foreach (Match match in tagMatches)
            {
                var tagName = match.Value.Substring(1).ToLower();               

                Tag tag = GetTag(tagName, provider, tokens);

                tags.Add(tag);
            }

            return tags;
        }
        catch (RegexMatchTimeoutException ex)
        {
            throw new Exception("An unexpected error occurred while checking tags existence", ex);
        }
    }

    public bool Equals(Tag tag1, Tag tag2)
    {
        var areEqual = string.Equals(tag1.Name, tag2.Name, StringComparison.OrdinalIgnoreCase);

        if (areEqual)
            tag1.TimeTokens += tag2.TimeTokens;

        return areEqual;
    }

    public int GetHashCode([DisallowNull] Tag obj)
    {
        return obj.Name.ToLower().GetHashCode();
    }
}

