using FluentEmail.Core;
using MyJournalist.App.Abstract;
using MyJournalist.Domain.Entity;
using MyJournalist.Email;
using MyJournalist.Email.Config.Abstract;
using MyJournalist.Worker.Config;
using TestWorker.Views;

namespace MyJournalist.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITagManager _tagManager;
    private readonly IRecordManager _recordManager;
    private readonly IDailyRecordsSetManager _dailyRecordsSetManager;
    private readonly IFluentEmail _fluentEmail;
    private readonly IEmailConfig _emailConfig;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly FileSystemWatcher _watcher;
    private readonly string _txtName;
    private readonly string _txtLocation;
    private readonly TimeSpan _firstInterval;
    private readonly TimeSpan _secondInterval;
    private readonly TimeSpan _startTime;
    private PeriodicTimer _periodicTimer;
    private bool _inProgress = false;

    public Worker(ILogger<Worker> logger,
                  ITagManager tagManager,
                  IRecordManager recordManager,
                  IDateTimeProvider dateTimeProvider,
                  IDailyRecordsSetManager dailyRecordsSetManager,
                  IFluentEmail fluentEmail,
                  IEmailConfig emailConfig,
                  IFileConfig config,
                  ITimerConfig timerConfig)
    {
        _logger = logger;
        _tagManager = tagManager;
        _recordManager = recordManager;
        _dateTimeProvider = dateTimeProvider;
        _dailyRecordsSetManager = dailyRecordsSetManager;
        _fluentEmail = fluentEmail;
        _emailConfig = emailConfig;
        _watcher = new FileSystemWatcher();

        string defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        string defaultName = "myNotes.txt";
        _txtLocation = string.IsNullOrWhiteSpace(config.TxtFileLocation) ? defaultPath : config.TxtFileLocation;
        _txtName = string.IsNullOrWhiteSpace(config.TxtName) ? defaultName : config.TxtName;

        _startTime = timerConfig.MeasurementTime;
        TimeSpan currentTime = _dateTimeProvider.Now.TimeOfDay;
        if (currentTime.CompareTo(_startTime) == 0)
            _startTime = _startTime.Add(TimeSpan.FromSeconds(1));

        _firstInterval = currentTime > _startTime ? (_startTime.Add(new TimeSpan(24, 0, 0)) - currentTime) : (_startTime - currentTime);

        _secondInterval = new TimeSpan(timerConfig.PeriodInHours, timerConfig.PeriodInMinutes, timerConfig.PeriodInSeconds);
        
        if (_secondInterval.CompareTo(new TimeSpan(0, 0, 0)) == 0)
            _secondInterval = _secondInterval.Add(TimeSpan.FromSeconds(1));

        _periodicTimer = new PeriodicTimer(_firstInterval);
        FileWatcherSetup();
    }
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await RunAllFilesCheckAsync();
        _watcher.EnableRaisingEvents = true;
        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _periodicTimer.WaitForNextTickAsync(stoppingToken);
        _periodicTimer.Dispose();
        _periodicTimer = new PeriodicTimer(_secondInterval);

        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
           await RunAllFilesCheckAsync();
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _watcher.EnableRaisingEvents = false;
        await RunAllFilesCheckAsync();
        await base.StopAsync(cancellationToken);
    }

    public override void Dispose()
    {
        _periodicTimer?.Dispose();
        _watcher?.Dispose();
        base.Dispose();
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
        await Task.Delay(200);
        RunTxtCheck();
        _inProgress &= false;
    }

    private async Task<List<int>> SendDailyRecSetListAsync(List<DailyRecordsSet> list)
    {
        List<int> tasks = new List<int>();

        foreach (var item in list)
        {
            tasks.Add(await DailyRecSetEmailAsync(item));
        }

        return tasks;
    }

    private async Task<int> DailyRecSetEmailAsync(DailyRecordsSet model, CancellationToken? stoppingToken = null)
    {
        var emailService = new EmailService<DailyRecordsSet>(_fluentEmail, _emailConfig);
        string subject = $"Automatic summary from the hour: {_startTime}";
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

    private async Task RunAllFilesCheckAsync()
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
                List<int> sendResults = await SendDailyRecSetListAsync(group.Value);
                foreach (var record in sendResults)
                {
                    //logowanie
                }
                _dailyRecordsSetManager.SaveListInFile(group.Value);
            }
        }

        List<Record> presentRecords = _recordManager.FindEqualDateRecords(recordsList, _dateTimeProvider.Now);
        _recordManager.ClearTxt();
        _recordManager.SaveListInFile(presentRecords);
        _tagManager.MergedTagsSave(tagsToSave);

    }
}
