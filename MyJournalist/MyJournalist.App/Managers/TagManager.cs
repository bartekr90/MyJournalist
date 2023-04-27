using MyJournalist.App.Abstract;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Managers;

public class TagManager : ITagManager
{
    private IFileCollectionService<Tag> _fileService;
    private IDateTimeProvider _dateTimeProvider;
    private ITagService _tagService;

    public TagManager(IDateTimeProvider dateTimeProvider,
                      IFileCollectionService<Tag> fileService,
                      ITagService tagService)
    {
        _dateTimeProvider = dateTimeProvider;
        _fileService = fileService;
        _tagService = tagService;
    }

    public void MargeAndSaveTagsInFile(ICollection<Tag> tags)
    {
        LoadTagsFromFile();

        List<Tag> newList = MergeTags(_tagService.GetAllItems(), tags);

        _fileService.ClearFile(_tagService.GetFileName());

        _fileService.UpdateFile(newList, _tagService.GetFileName());
    }


    public List<Tag> MergeTags(ICollection<Tag>? primaryTags, ICollection<Tag>? newTags)
    {
        return _tagService.MakeUnion(primaryTags, newTags);
    }

    public void LoadTagsFromFile()
    {

        if (CheckFileExists())
        {
            var list = _fileService.ReadFile(_tagService.GetFileName()).ToList() ?? new List<Tag>();
            _tagService.SetItems(list);
        }
        else
            _fileService.CreateFile(_tagService.GetFileName());
    }

    public List<Tag> GetTagsFromContent(string contentFromTxt)
    {
        uint tokens = _tagService.FindTokens(contentFromTxt);
        List<Tag> tags = _tagService.FindAllTags(contentFromTxt, _dateTimeProvider, tokens);
        return tags;
    }

    public List<Tag> MergeTagsFromRecordList(List<Record> records)
    {
        if (records == null || records.Count == 0)
            return new List<Tag>();

        List<Tag> primaryTags = records.FirstOrDefault().Tags?.ToList() ?? new List<Tag>();

        foreach (Record record in records)
        {
            primaryTags = MergeTags(primaryTags, record.Tags);
        }
        return primaryTags;
    }

    private bool CheckFileExists()
    {
        return _fileService.CheckFileExists(_tagService.GetFileName());
    }
}

