using MyJournalist.App.Abstract;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Managers;

public class DailyRecordsSetManager : IDailyRecordsSetManager
{
    private IDateTimeProvider _dateTimeProvider;
    private IFileCollectionService<DailyRecordsSet> _fileService;
    private IDailyRecordsSetService _dailySetService;

    public DailyRecordsSetManager(IDateTimeProvider dateTimeProvider,
                                  IFileCollectionService<DailyRecordsSet> fileService,
                                  IDailyRecordsSetService dailySetService)
    {
        _dateTimeProvider = dateTimeProvider;
        _fileService = fileService;
        _dailySetService = dailySetService;
    }

    public Dictionary<(int year, int month), List<DailyRecordsSet>> GroupDailySetsByMonth(List<DailyRecordsSet> dailySets)
    {
        var result = new Dictionary<(int year, int month), List<DailyRecordsSet>>();

        if (dailySets == null)
            return result;

        var dailySetsByMonth = dailySets
            .GroupBy(drs => new { drs.RefersToDate.Year, drs.RefersToDate.Month });

        foreach (var group in dailySetsByMonth)
        {
            result.Add((group.Key.Year, group.Key.Month), group.ToList());
        }

        return result;
    }

    public void LoadDailySetsFromFile(DateTimeOffset date)
    {
        _dailySetService.RefersToDate = date;

        if (_fileService.CheckFileExists(_dailySetService.GetFileName()))
        {
            var list = _fileService.ReadFile(_dailySetService.GetFileName()).ToList() ?? new List<DailyRecordsSet>();
            _dailySetService.SetItems(list);
        }
        else
            _fileService.CreateFile(_dailySetService.GetFileName());
    }
   
    public void SetDateForService(DateTimeOffset date)
    {
        _dailySetService.RefersToDate = date;
    }

    public DailyRecordsSet GetDailyRecordsSet(ICollection<Record>? records, ICollection<Tag>? tags)
    {
        if (!(records == null || records.Count == 0))
            return _dailySetService.GetDailyRecordsSet(records, tags, _dateTimeProvider);
        else
            return _dailySetService.GetEmptyObj(_dateTimeProvider);
    }

    public void SaveListInFile(ICollection<DailyRecordsSet> setsFromMonth)
    {
        DailyRecordsSet? set = setsFromMonth.FirstOrDefault();
        if (set == null)
            return;

        LoadDailySetsFromFile(set.RefersToDate);

        List<DailyRecordsSet> setsToSave = _dailySetService.MakeUnion(_dailySetService.GetAllItems(), setsFromMonth);

        _fileService.ClearFile(_dailySetService.GetFileName());

        _fileService.UpdateFile(setsToSave, _dailySetService.GetFileName());
    }
}

