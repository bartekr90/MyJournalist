using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.App.Concrete;
using MyJournalist.App.Config;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;
using MyJournalist.Email.Config;
using MyJournalist.Email.Config.Abstract;
using MyJournalist.Worker;
using MyJournalist.Worker.Config;
using System.ComponentModel.DataAnnotations;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        var fileConfig = ValidateConfig<FileConfig>(host, "FileConfig");
        services.AddSingleton<IFileConfig>(servicePovider => fileConfig);

        var timerConfig = ValidateConfig<TimerConfig>(host, "TimerConfig");
        services.AddSingleton<ITimerConfig>(servicePovider => timerConfig);

        var smtpConfig = ValidateConfig<EmailSmtpConfig>(host, "EmailSmtpConfig");
        services.AddSingleton<IEmailSmtpConfig>(servicePovider => smtpConfig);

        var emailConfig = ValidateConfig<EmailConfig>(host, "EmailConfig");
        services.AddSingleton<IEmailConfig>(servicePovider => emailConfig);
       
        services.AddFluentEmail(smtpConfig.SenderEmail)
        .AddRazorRenderer()
        .AddSmtpSender(smtpConfig.GetClient());

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

static T ValidateConfig<T>(HostBuilderContext host, string section) where T : new()
{
    T config = host.Configuration.GetSection(section).Get<T>() ?? new T();

    var results = new List<ValidationResult>();
    bool valid = Validator.TryValidateObject(config, new ValidationContext(config), results, true);

    if (!valid)
    {
        foreach (var validationResult in results)
        {
            Console.WriteLine(validationResult.ErrorMessage);
        }
        throw new Exception("Not all the settings are valid");
    }

    return config;
}
