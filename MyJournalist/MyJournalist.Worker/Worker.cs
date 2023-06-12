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
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        //RunAllFilesCheck();
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            //RunTxtCheck();

            var TestDailySetList = new List<DailyRecordsSet>
            {
                new DailyRecordsSet()
                {
                     Id = 2,
                    CreatedById = 123,
                    CreatedDateTime = new DateTimeOffset(2023, 4, 4, 9, 0, 0, TimeSpan.Zero),
                    ModifiedById = 123,
                    ModifiedDateTime = new DateTimeOffset(2023, 4, 4, 9, 0, 0, TimeSpan.Zero),
                    RefersToDate = new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero),
                    MergedTags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "trzeci",
                            Id = 1,
                        },
                        new Tag
                        {
                            Name = "pierwszy",
                            Id = 2,
                        },
                        new Tag
                        {
                            Name = "opis",
                            Id = 4,
                        }

                    },
                    MergedContents = "Przyk³adowy opis 2 #opis\r\n***\r\nOpis 3 #opis\r\n***\r\nTekst opisowy 1 #pierwszy\r\n***\r\nInny tekst 3 #trzeci\r\n***\r\n\r\n",
                    EmailHasBeenSent = false,
                    HasAnyRecords = true,
                    HasAnyTags = true
                },

                new DailyRecordsSet()
                {
                     Id = 1,
                    CreatedById = 123,
                    CreatedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    ModifiedById = 123,
                    ModifiedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    RefersToDate = new DateTimeOffset(2023, 5, 11, 0, 0, 0, TimeSpan.Zero),
                    MergedTags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "trzeci",
                            Id = 1,
                            TimeTokens = 5
                        },
                        new Tag
                        {
                            Name = "tekst",
                            Id = 2,
                            TimeTokens = 5
                        },
                        new Tag
                        {
                            Name = "opis",
                            Id = 4,
                            TimeTokens = 5
                        },
                        new Tag
                        {
                            Name = "przyk³ad",
                            Id = 3,
                            TimeTokens = 5
                        }
                    },
                    MergedContents = "Przyk³adowy $5 tekst 3 #trzeci\r\n***\r\nTekst 3 $5#tekst\r\n***\r\n$5Przyk³ad 3 #przyk³ad\r\n***\r\nInny opis 1 #opis$5\r\n***\r\n\r\n",
                    EmailHasBeenSent = false,
                    HasAnyRecords = true,
                    HasAnyTags = true
                },

                new DailyRecordsSet()
                {
                     Id = 3,
                    CreatedById = 123,
                    CreatedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    ModifiedById = 123,
                    ModifiedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    RefersToDate = new DateTimeOffset(2023, 5, 11, 0, 0, 0, TimeSpan.Zero),
                    MergedContents = "Przyk³adowy teskt daohdoaishdoaihdoahd",
                    EmailHasBeenSent = false,
                    HasAnyRecords = true,
                    HasAnyTags = false
                },
                new DailyRecordsSet()

            };
            List<int> failCount = new List<int>();
            for (int i = 0; i < TestDailySetList.Count; i++)
            {
                Console.Out.WriteLine("trwa wysy³¹nie");

                failCount[i] = await DailyRecSetEmailAsync(TestDailySetList[i], stoppingToken);

                Console.Out.WriteLine("wys³ano wiadomosc");

                await Task.Delay(500, stoppingToken);

            }
            foreach (var item in failCount)
            {
                if (item == 1)
                     Console.Out.WriteLine("wys³ano wiadomosc");
                else
                     Console.Out.WriteLine("Error");
            }

            await Task.Delay(15000, stoppingToken);
        }
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        //RunAllFilesCheck();
        return base.StopAsync(cancellationToken);
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
