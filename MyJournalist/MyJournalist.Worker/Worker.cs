using MyJournalist.Domain.Entity;

namespace MyJournalist.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private ITagManager _tagManager;
    private IRecordManager _recordManager;
    private IDailyRecordsSetManager _dailyRecordsSetManager;
    private IDateTimeProvider _dateTimeProvider;

    public Worker(ILogger<Worker> logger,
                  ITagManager tagManager,
                  IRecordManager recordManager,
                  IDateTimeProvider dateTimeProvider,
                  IDailyRecordsSetManager dailyRecordsSetManager)
    {
        _logger = logger;
        _tagManager = tagManager;
        _recordManager = recordManager;
        _dateTimeProvider = dateTimeProvider;
        _dailyRecordsSetManager = dailyRecordsSetManager;
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        RunAllFilesCheck();
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            RunTxtCheck();
            await Task.Delay(15000, stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        RunAllFilesCheck();
        return base.StopAsync(cancellationToken);
    }

    private void RunTxtCheck()
    {
        string content = _recordManager.GetDataFromTxt();
        List<Tag> tags = _tagManager.GetTagsFromContent(content);
        _recordManager.SaveRecordInFile(_recordManager.GetRecord(content, tags));
        _recordManager.ClearTxt();
    }

    private void RunAllFilesCheck()
    {
        List<Tag> tagsToSave = new List<Tag>();
        string content = _recordManager.GetDataFromTxt();
        List<Tag> tags = _tagManager.GetTagsFromContent(content);
        List<Record> recordsList = _recordManager.MakeNewRecordList(_recordManager.GetRecord(content, tags));
        List<Record> pastRecords = _recordManager.FindPastDateRecords(recordsList, _dateTimeProvider.Now);

        if (!(pastRecords == null || pastRecords.Count == 0))
        {
            List<DailyRecordsSet> newDailyRecordsSetsList = new();
            IEnumerable<IGrouping<DateTime, Record>> recordsGroupedByMonth = _recordManager.GroupRecordsByDate(pastRecords);

            foreach (IGrouping<DateTime, Record> group in recordsGroupedByMonth)
            {
                List<Record> fullRecords = _recordManager.GeRecordsWithContent(group.ToList());
                _dailyRecordsSetManager.SetDateForService(group.Key);
                List<Tag> tagsForDailySet = _tagManager.MergeTagsFromRecordList(fullRecords);
                DailyRecordsSet dailyRecordSet = _dailyRecordsSetManager.GetDailyRecordsSet(fullRecords, tagsForDailySet);
                newDailyRecordsSetsList.Add(dailyRecordSet);
                tagsToSave = _tagManager.MergeTags(tagsToSave, tagsForDailySet);
            }

            var dailySetsGroupedByMonth = _dailyRecordsSetManager.GroupDailySetsByMonth(newDailyRecordsSetsList);

            foreach (var group in dailySetsGroupedByMonth)
            {
                _dailyRecordsSetManager.SaveListInFile(group.Value);
            }
        }

        List<Record> presentRecords = _recordManager.FindEqualDateRecords(recordsList, _dateTimeProvider.Now);
        _recordManager.ClearTxt();
        _recordManager.SaveListInFile(presentRecords);
        _tagManager.MargeAndSaveTagsInFile(tagsToSave);

    }
}
