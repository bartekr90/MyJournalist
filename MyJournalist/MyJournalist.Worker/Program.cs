using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;
using MyJournalist.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ITxtFileService, TxtFileService>();

        services.AddSingleton<IFileCollectionService<Record>, JsonFileService<Record>>();
        services.AddTransient<IRecordService, RecordService>();
        services.AddTransient<IRecordManager, RecordManager>();

        services.AddSingleton<IFileCollectionService<DailyRecordsSet>, JsonFileService<DailyRecordsSet>>();
        services.AddTransient<IDailyRecordsSetService, DailyRecordsSetService>();
        services.AddTransient<IDailyRecordsSetManager, DailyRecordsSetManager>();

        services.AddSingleton<IFileCollectionService<Tag>, JsonFileService<Tag>>();
        services.AddTransient<ITagService, TagService>();
        services.AddTransient<ITagManager, TagManager>();
    })
    .Build();

host.Run();
