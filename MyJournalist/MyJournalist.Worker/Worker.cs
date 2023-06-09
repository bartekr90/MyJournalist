using MyJournalist.App.Abstract;
using MyJournalist.Domain.Entity;
using MyJournalist.Email.Abstract;
using MyJournalist.Worker.Config;
using MyJournalist.Worker.Views;

namespace MyJournalist.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITagManager _tagManager;
    private readonly IRecordManager _recordManager;
    private readonly IDailyRecordsSetManager _dailyRecordsSetManager;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEmailService<DailyRecordsSet> _dailyRecordsSetEmail;
    private readonly IEmailService<Record> _recordEmail;
    private readonly FileSystemWatcher _watcher;
    private readonly string _recordSubject;
    private readonly string _dailyRecordsSetSubject;
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
                  IFileConfig fileConfig,
                  ITimerConfig timerConfig,
                  IEmailService<DailyRecordsSet> dailyRecordsSetEmail,
                  IEmailService<Record> recordEmail,
                  IConfiguration config)
    {
        _logger = logger;
        _tagManager = tagManager;
        _recordManager = recordManager;
        _dateTimeProvider = dateTimeProvider;
        _dailyRecordsSetManager = dailyRecordsSetManager;
        _dailyRecordsSetEmail = dailyRecordsSetEmail;
        _recordEmail = recordEmail;
        _watcher = new FileSystemWatcher();

        _recordSubject = config.GetSection("EmailConfig")?.GetValue<string>("RecordSubject") ?? "";
        _dailyRecordsSetSubject = config.GetSection("EmailConfig")?.GetValue<string>("DailyRecordsSetSubject") ?? "";

        string defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        string defaultName = "myNotes.txt";
        _txtLocation = string.IsNullOrWhiteSpace(fileConfig.TxtFileLocation) ? defaultPath : fileConfig.TxtFileLocation;
        _txtName = string.IsNullOrWhiteSpace(fileConfig.TxtName) ? defaultName : fileConfig.TxtName;

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
        await base.StartAsync(cancellationToken);
        await Console.Out.WriteLineAsync("Rozpocz�to prac�");
        _watcher.EnableRaisingEvents = true;

        await FromTextToTempFile();
        await Console.Out.WriteLineAsync($"Zako�czono sprawdzenie Txt.");
        await Console.Out.WriteLineAsync($"Raportowanie rozpocznie si� o: {_startTime}");
        await Console.Out.WriteLineAsync($"Raportowanie b�dzie wykonywane co: {_secondInterval}");
        await Console.Out.WriteLineAsync($"Do rozpocz�cia pozosta�o: {_firstInterval}");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _periodicTimer.WaitForNextTickAsync(stoppingToken);
        _periodicTimer.Dispose();

        await Console.Out.WriteLineAsync("Trwa spawdzanie pliku i wysy�anie wiadomo�ci");
        await FromTempToArchiveFileAsync();
        await Console.Out.WriteLineAsync("Zako�czono sprawdzanie plik�w i wysy�anie wiadomo�ci");

        _periodicTimer = new PeriodicTimer(_secondInterval);

        await Console.Out.WriteLineAsync($"Nast�pny raport zostanie wykonany za {_secondInterval}");

        while (await _periodicTimer.WaitForNextTickAsync(stoppingToken)
            && !stoppingToken.IsCancellationRequested)
        {
            await Console.Out.WriteLineAsync("Trwa spawdzanie pliku i wysy�anie wiadomo�ci");

            await FromTempToArchiveFileAsync();

            await Console.Out.WriteLineAsync("Zako�czono sprawdzanie plik�w i wysy�anie wiadomo�ci");
            await Console.Out.WriteLineAsync($"Nast�pny raport zostanie wykonany za {_secondInterval}");

        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await Console.Out.WriteLineAsync("Zamykanie aplikacji. Trwa spawdzanie pliku i wysy�anie wiadomo�ci");
        _watcher.EnableRaisingEvents = false;
        await FromTempToArchiveFileAsync();
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
        await FromTextToTempFile();
        _inProgress &= false;
    }

    private async Task<List<int>> SendDailyRecSetListAsync(List<DailyRecordsSet> list, CancellationToken? stoppingToken = null)
    {
        List<int> tasks = new List<int>();

        foreach (var item in list)
        {
            var res = await DailyRecSetEmailAsync(item, stoppingToken);
            tasks.Add(res);

            if (res == 1)
                item.EmailHasBeenSent = true;
        }
        return tasks;
    }

    private async Task<int> DailyRecSetEmailAsync(DailyRecordsSet model, CancellationToken? stoppingToken = null)
    {
        string plainBody = GeneratePlainText.GetDailyRecordsSetBody(model);
        return await _dailyRecordsSetEmail.SendEmailAsync(model, _dailyRecordsSetSubject, plainBody, stoppingToken);
    }
    private async Task<int> RecordEmailAsync(Record model, CancellationToken? stoppingToken = null)
    {
        string plainBody = GeneratePlainText.GetRecordBody(model);
        return await _recordEmail.SendEmailAsync(model, _recordSubject, plainBody, stoppingToken);
    }

    public async Task FromTextToTempFile()
    {
        var rec = GetRecordFromText();

        if (rec.Tags is not null && rec.Tags.Any(tag => tag.Name == "sendnow"))
        {
            var result = await RecordEmailAsync(rec);

            if (result == 1)
                await Console.Out.WriteLineAsync("wys�ano wiadomo�c");
            else
                await Console.Out.WriteLineAsync("nie wys�ano");
        }
        _recordManager.SaveRecordInFile(rec);
        _recordManager.ClearTxt();
    }

    public async Task FromTempToArchiveFileAsync()
    {
        Record rec = GetRecordFromText();
        List<Record> recordsList = _recordManager.MakeNewRecordList(rec);
        List<DailyRecordsSet> newDailyRecordsSetsList = GetDailyRecordsSets(recordsList);

        List<int> sendResults = await SendDailyRecSetListAsync(newDailyRecordsSetsList);
        foreach (var record in sendResults)
        {
            if (record == 1)
                await Console.Out.WriteLineAsync("Wys�ano wiadomo��");
            else
                await Console.Out.WriteLineAsync("Nie wys�ano wiadomo�ci");
        }

        var dailySetsGroupedByMonth = _dailyRecordsSetManager.GroupDailySetsByMonth(newDailyRecordsSetsList);

        foreach (var group in dailySetsGroupedByMonth)
        {
            _dailyRecordsSetManager.SaveListInFile(group.Value);
        }

        List<Tag> tagsToSave = new List<Tag>();
        foreach (var item in newDailyRecordsSetsList)
        {
            tagsToSave = _tagManager.MergeTags(tagsToSave, item.MergedTags);
        }

        _recordManager.ClearTxt();
        _recordManager.ClearTempFile();
        _tagManager.MergedTagsSave(tagsToSave);
    }
    
    private List<DailyRecordsSet> GetDailyRecordsSets(List<Record> recordsList)
    {
        List<DailyRecordsSet> newDailyRecordsSetsList = new();
        IEnumerable<IGrouping<DateTime, Record>> recordsGroupedByMonth = _recordManager.GroupRecordsByDate(recordsList);

        foreach (IGrouping<DateTime, Record> group in recordsGroupedByMonth)
        {
            List<Record> allRecordsFromMonth = _recordManager.GetRecordsWithContent(group.ToList());
            _dailyRecordsSetManager.SetDateForService(group.Key);
            List<Tag> tagsForDailySet = _tagManager.MergeTagsFromRecords(allRecordsFromMonth);
            DailyRecordsSet dailyRecordSet = _dailyRecordsSetManager.GetDailyRecordsSet(allRecordsFromMonth, tagsForDailySet);
            newDailyRecordsSetsList.Add(dailyRecordSet);
        }

        return newDailyRecordsSetsList;       

    }

    private Record GetRecordFromText()
    {
        string content = _recordManager.GetDataFromTxt();
        List<Tag> tags = _tagManager.GetTagsFromContent(content);
        return _recordManager.GetRecord(content, tags);
    }

}
