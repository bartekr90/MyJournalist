using MyJournalist.App.Abstract;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Managers;

public class RecordManager : IRecordManager
{
    private IDateTimeProvider _dateTimeProvider;
    private IRecordService _recordService;
    private ITxtFileService _txtService;
    private IFileCollectionService<Record> _fileService;

    public RecordManager(IDateTimeProvider provider,
                         ITxtFileService txtService,
                         IFileCollectionService<Record> fileService,
                         IRecordService recordService)
    {
        _dateTimeProvider = provider;
        _txtService = txtService;
        _fileService = fileService;
        _recordService = recordService;
    }

    public string GetDataFromTxt()
    {
        _dateTimeProvider.DateOfContent = _txtService.GetGetLastWriteTime();
        return _txtService.ReadData();
    }
    public Record GetRecord(string contentFromTxt, ICollection<Tag> tags)
    {
        Record record;

        if (string.IsNullOrWhiteSpace(contentFromTxt))
            record = _recordService.GetEmptyObj(_dateTimeProvider);
        else
            record = _recordService.GetRecordFromContent(contentFromTxt, tags, _dateTimeProvider);

        return record;
    }
    public List<Record> MakeNewRecordList(Record record)
    {
        LoadRecordsFromFile();
        var list = _recordService.GetAllItems();
        list.Add(record);
        return list;
    }

    public void SaveListInFile(List<Record> listToSave)
    {
        if (listToSave.Count > 0)
        {
            _fileService.ClearFile(_recordService.GetFileName());
            _fileService.UpdateFile(listToSave, _recordService.GetFileName());
        }
    }

    public void SaveRecordInFile(Record record)
    {
        LoadRecordsFromFile();
        List<Record> list = _recordService.GetAllItems();
        list.Add(record);

        _fileService.ClearFile(_recordService.GetFileName());
        _fileService.UpdateFile(list, _recordService.GetFileName());
    }

    public void ClearTxt()
    {
        _txtService.ClearData();
    }

    public List<Record> FindPastDateRecords(ICollection<Record> list, DateTimeOffset compareDate)
    {
        var records = list
           .Where(r => r.ContentDate.Date != compareDate.Date)
           .ToList();

        return records;
    }

    public List<Record> FindEqualDateRecords(ICollection<Record> list, DateTimeOffset compareDate)
    {
        var records = list
            .Where(r => r.ContentDate.Date == compareDate.Date)
            .ToList();

        return records;
    }
    public IEnumerable<IGrouping<DateTime, Record>> GroupRecordsByDate(ICollection<Record> records)
    {
        IEnumerable<IGrouping<DateTime, Record>> recordsByDate = records.GroupBy(r => r.ContentDate.Date);
        return recordsByDate;
    }

    public List<Record> GeRecordsWithContent(List<Record> group)
    {
        List<Record> list = group
        .Where(r => r.HasContent == true)
        .ToList();
        return list;
    }

    public void LoadRecordsFromFile()
    {
        if (CheckFileExists())
        {
            var list = _fileService.ReadFile(_recordService.GetFileName()).ToList() ?? new List<Record>();
            _recordService.SetItems(list);
        }
        else
            _fileService.CreateFile(_recordService.GetFileName());
    }
    private bool CheckFileExists()
    {
        return _fileService.CheckFileExists(_recordService.GetFileName());
    }

}

