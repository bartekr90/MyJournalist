using FluentEmail.Core;
using MyJournalist.App.Abstract;
using MyJournalist.Domain.Entity;
using MyJournalist.Email;
using TestWorker.Views;

namespace MyJournalist.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITagManager _tagManager;
    private readonly IRecordManager _recordManager;
    private readonly IDailyRecordsSetManager _dailyRecordsSetManager;
    private readonly IFluentEmail _fluentEmail;
    private readonly IConfiguration _config;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly FileSystemWatcher _watcher;
    private readonly string _txtName;
    private readonly string _txtLocation;
    private bool _inProgress = false;

    public Worker(ILogger<Worker> logger,
                  ITagManager tagManager,
                  IRecordManager recordManager,
                  IDateTimeProvider dateTimeProvider,
                  IDailyRecordsSetManager dailyRecordsSetManager,
                  IFluentEmail fluentEmail,
                  IConfiguration config)
    {
        _logger = logger;
        _tagManager = tagManager;
        _recordManager = recordManager;
        _dateTimeProvider = dateTimeProvider;
        _dailyRecordsSetManager = dailyRecordsSetManager;
        _fluentEmail = fluentEmail;
        _config = config;
        _watcher = new FileSystemWatcher();
        _txtName = config["DirSettings:TxtName"] ?? "*.txt";
        _txtLocation = config["DirSettings:TxtFileLocation"] ?? Path.Combine(Directory.GetCurrentDirectory(), "Data");
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        FileWatcherSetup();
        // RunAllFilesCheck();
        _watcher.EnableRaisingEvents = true;
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //RunTxtCheck(); - tego nie porzebujê bo mam event
            //zrobic metody async dla plikow

            await Task.Delay(1500000, stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher.EnableRaisingEvents = false;
        _watcher.Dispose();
        // RunAllFilesCheck();
        return base.StopAsync(cancellationToken);
    }

    private void FileWatcherSetup()
    {
        _watcher.Path = _txtLocation;
        _watcher.Filter = _txtName;
        _watcher.NotifyFilter = NotifyFilters.Size;
        _watcher.Changed += OnChanged;

    }

    private async void OnChanged(object sender, FileSystemEventArgs e)
    {
        string filePath = Path.Combine(_txtLocation, _txtName);
        FileInfo fileInfo = new FileInfo(filePath);

        if (!(fileInfo.Length > 0) || _inProgress)
            return;

        _inProgress = true;
        await Task.Delay(300);
        RunTxtCheck();
        _inProgress &= false;
    }

    public async Task<List<int>> RunParallelAsync(List<DailyRecordsSet> list)
    {
        List<int> tasks = new List<int>();

        foreach (var item in list)
        {
            tasks.Add(await DailyRecSetEmailAsync(item));
        }

        return tasks;
    }

    private int DailyRecSetEmail(DailyRecordsSet model)
    {
        var emailService = new EmailService<DailyRecordsSet>(_fluentEmail, _config);
        string subject = "Automatyczne podsumowanie";
        string plainBody = GeneratePlainText.GetDailyRecordsSetBody(model);
        return emailService.SendEmail(model, subject, plainBody);
    }

    private async Task<int> DailyRecSetEmailAsync(DailyRecordsSet model, CancellationToken? stoppingToken = null)
    {
        var emailService = new EmailService<DailyRecordsSet>(_fluentEmail, _config);
        string subject = "Automatyczne podsumowanie";
        string plainBody = GeneratePlainText.GetDailyRecordsSetBody(model);
        return await emailService.SendEmailAsync(model, subject, plainBody, stoppingToken);
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
                List<Record> fullRecords = _recordManager.GetRecordsWithContent(group.ToList());
                _dailyRecordsSetManager.SetDateForService(group.Key);
                List<Tag> tagsForDailySet = _tagManager.MergeTagsFromRecords(fullRecords);
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
        _tagManager.MergedTagsSave(tagsToSave);

    }
}
