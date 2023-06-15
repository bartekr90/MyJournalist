using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;
using MyJournalist.Email;
using MyJournalist.Worker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((host, services) =>
    {
        SetEmailSetup(host);

        services
        .AddFluentEmail(EmailSetup.SenderEmail)
        .AddRazorRenderer()
        .AddSmtpSender(EmailSetup.GetClient());

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

static void SetEmailSetup(HostBuilderContext host)
{
    var emailSettings = host.Configuration.GetSection("EmailSettings");

    int port;
    if (!int.TryParse(emailSettings["Port"], out port))
        port = 25;

    int portLocal;
    if (!int.TryParse(emailSettings["PortLocal"], out portLocal))
        portLocal = 25;

    bool enableSsl;
    if (!bool.TryParse(emailSettings["EnableSsl"], out enableSsl))
        enableSsl = false;

    bool enableSslLocal;
    if (!bool.TryParse(emailSettings["EnableSslLocal"], out enableSslLocal))
        enableSslLocal = false;

    bool localSettings;
    if (!bool.TryParse(emailSettings["UseLocalSettings"], out localSettings))
        localSettings = true;

    bool defaultCredentials;
    if (!bool.TryParse(emailSettings["UseDefaultCredentials"], out defaultCredentials))
        defaultCredentials = false;

    EmailSetup.HostSmtp = emailSettings["HostSmtp"] ?? "localhost";
    EmailSetup.EnableSsl = enableSsl;
    EmailSetup.Port = port;
    EmailSetup.SenderEmail = emailSettings["SenderEmail"] ?? "defaultEmail";
    EmailSetup.SenderEmailPassword = emailSettings["SenderEmailPassword"] ?? "defaultPassword";
    EmailSetup.DefaultCredentials = defaultCredentials;
    EmailSetup.UseLocalSettings = localSettings;
    EmailSetup.HostSmtpLocal = emailSettings["HostSmtpLocal"] ?? "localhost";
    EmailSetup.EnableSslLocal = enableSslLocal;
    EmailSetup.PortLocal = portLocal;
}

