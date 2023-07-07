using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;
using MyJournalist.Email.Abstract;
using MyJournalist.Worker.Config;

namespace MyJournalist.Worker.Tests;

[Collection("WorkerCollection")]
public class WorkerTests
{
    private const string _FILE_PATH = "TestData";
    private readonly string _currectDir;
    private readonly string _fullPath;

    public WorkerTests()
    {
        _currectDir = Directory.GetCurrentDirectory();
        _fullPath = Path.GetRelativePath(_currectDir, _FILE_PATH);
    }

    public static TheoryData<DateTimeOffset[], string[]> FromTextToTempFile_ShouldSendEmail =>
        new TheoryData<DateTimeOffset[], string[]>
        {
            {DateArray, contentArrayWithTags},
        };

    [Theory]
    [MemberData(nameof(FromTextToTempFile_ShouldSendEmail))]
    public async Task FromTextToTempFile_ShouldSendEmail_WhenCalledAsync(DateTimeOffset[] dates, string[] contents)
    {
        // Arrange
        string txtName = "Notes.txt";
        string json24Name = "Records_from_last_24_h.json";

        var mockLogger = new Mock<ILogger<Worker>>();
        var mockDailyRecordsSetManager = new Mock<IDailyRecordsSetManager>();
        var mockTimerConfig = new Mock<ITimerConfig>();
        var mockConfig = new Mock<IConfiguration>();

        var mockDailyRecordsSetEmail = new Mock<IEmailService<DailyRecordsSet>>();
        mockDailyRecordsSetEmail.Setup(m => m.SendEmailAsync(It.IsAny<DailyRecordsSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()))
               .ReturnsAsync(1);
        var mockRecordEmail = new Mock<IEmailService<Domain.Entity.Record>>();
        mockRecordEmail.Setup(m => m.SendEmailAsync(It.IsAny<Domain.Entity.Record>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()))
               .ReturnsAsync(1);

        var mockDateTimeProvider = new Mock<IDateTimeProvider>();

        var mockFileConfig = new Mock<IFileConfig>();
        mockFileConfig.Setup(m => m.JsonFileLocation).Returns(_fullPath);
        mockFileConfig.Setup(m => m.TxtFileLocation).Returns(_fullPath);
        mockFileConfig.Setup(m => m.TxtName).Returns(txtName);

        // Real instances
        var tagsJsonFile = new JsonFileService<Tag>(mockFileConfig.Object);
        var txtFile = new TxtFileService(mockFileConfig.Object);
        var recordJsonFile = new JsonFileService<Domain.Entity.Record>(mockFileConfig.Object);
        var dailySetJsonFile = new JsonFileService<DailyRecordsSet>(mockFileConfig.Object);
        var recordService = new RecordService();

        var tagService = new TagService();
        var dailySetService = new DailyRecordsSetService();

        var dailySetManager = new DailyRecordsSetManager(mockDateTimeProvider.Object, dailySetJsonFile, dailySetService);
        var tagManager = new TagManager(mockDateTimeProvider.Object, tagsJsonFile, tagService);
        var recordManager = new RecordManager(mockDateTimeProvider.Object, txtFile, recordJsonFile, recordService);

        var worker = new Worker(mockLogger.Object,
                                tagManager,
                                recordManager,
                                mockDateTimeProvider.Object,
                                dailySetManager,
                                mockFileConfig.Object,
                                mockTimerConfig.Object,
                                mockDailyRecordsSetEmail.Object,
                                mockRecordEmail.Object,
                                mockConfig.Object);

        var expectedRecords = new List<Domain.Entity.Record> { GetRecorsArray[4], GetRecorsArray[5], GetRecorsArray[6] };

        var txtPath = Path.Combine(_fullPath, txtName);
        var json24Path = Path.Combine(_fullPath, json24Name);

        // Act

        for (int i = 5; i < 8; i++)
        {
            mockDateTimeProvider.Setup(m => m.DateOfContent).Returns(dates[i * 2 - 1]);
            mockDateTimeProvider.Setup(m => m.Now).Returns(dates[2 * i]);
            File.AppendAllText(Path.Combine(_fullPath, txtName), contents[i - 1]);

            await worker.FromTextToTempFile();
        }

        var result = recordJsonFile.ReadFile(recordService.GetFileName());
        var txtLenght = new FileInfo(txtPath).Length;
               
        if (File.Exists(json24Path))
            File.Delete(json24Path);

        // Assert
        mockRecordEmail.Verify(m => m.SendEmailAsync(
            It.IsAny<Domain.Entity.Record>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()), Times.Once);
        result.Should().BeEquivalentTo(expectedRecords);
        txtLenght.Should().Be(0);
    }

    public static TheoryData<DateTimeOffset[], string[]> FromTempToArchiveFileAsync =>
       new TheoryData<DateTimeOffset[], string[]>
       {
            {DateArray, contentArrayWithTags},
       };

    [Theory]
    [MemberData(nameof(FromTempToArchiveFileAsync))]
    public async Task FromTextToTempFile_FromTempToArchiveFileAsync_Test1(DateTimeOffset[] dates, string[] contents)
    {
        // Arrange
        string txtName = "notes.txt";
        string json24Name = "Records_from_last_24_h.json";
        string tagsName = "Tags_list.json";
        string dailySetsName = "RecordBook_lipiec_2023.json";

        var mockLogger = new Mock<ILogger<Worker>>();
        var mockDailyRecordsSetManager = new Mock<IDailyRecordsSetManager>();
        var mockTimerConfig = new Mock<ITimerConfig>();
        var mockConfig = new Mock<IConfiguration>();

        var mockDailyRecordsSetEmail = new Mock<IEmailService<DailyRecordsSet>>();
        mockDailyRecordsSetEmail.Setup(m => m.SendEmailAsync(It.IsAny<DailyRecordsSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()))
               .ReturnsAsync(1);
        var mockRecordEmail = new Mock<IEmailService<Domain.Entity.Record>>();

        var mockDateTimeProvider = new Mock<IDateTimeProvider>();

        var mockFileConfig = new Mock<IFileConfig>();
        mockFileConfig.Setup(m => m.JsonFileLocation).Returns(_fullPath);
        mockFileConfig.Setup(m => m.TxtFileLocation).Returns(_fullPath);
        mockFileConfig.Setup(m => m.TxtName).Returns(txtName);

        // Real instances
        var tagsJsonFile = new JsonFileService<Tag>(mockFileConfig.Object);
        var txtFile = new TxtFileService(mockFileConfig.Object);
        var recordJsonFile = new JsonFileService<Domain.Entity.Record>(mockFileConfig.Object);
        var dailySetJsonFile = new JsonFileService<DailyRecordsSet>(mockFileConfig.Object);
        var recordService = new RecordService();

        var tagService = new TagService();
        var dailySetService = new DailyRecordsSetService();

        var dailySetManager = new DailyRecordsSetManager(mockDateTimeProvider.Object, dailySetJsonFile, dailySetService);
        var tagManager = new TagManager(mockDateTimeProvider.Object, tagsJsonFile, tagService);
        var recordManager = new RecordManager(mockDateTimeProvider.Object, txtFile, recordJsonFile, recordService);

        var worker = new Worker(mockLogger.Object,
                                tagManager,
                                recordManager,
                                mockDateTimeProvider.Object,
                                dailySetManager,
                                mockFileConfig.Object,
                                mockTimerConfig.Object,
                                mockDailyRecordsSetEmail.Object,
                                mockRecordEmail.Object,
                                mockConfig.Object);

        var expectedDailySets = GetDailyRecordsSets;

        var expectedRecords = GetRecorsList;
      
        var expectedTags = new List<Tag> { GetTagsArray2[0], GetTagsArray[1], GetTagsArray[2] };

        var txtPath = Path.Combine(_fullPath, txtName);
        var json24Path = Path.Combine(_fullPath, json24Name);
        var tagsPath = Path.Combine(_fullPath, tagsName);
        var dailySetsPath = Path.Combine(_fullPath, dailySetsName);

        // Act
        for (int i = 0; i < 4; i++)
        {
            mockDateTimeProvider.Setup(m => m.DateOfContent).Returns(dates[i * 2]);
            mockDateTimeProvider.Setup(m => m.Now).Returns(dates[2 * i + 1]);
            File.AppendAllText(Path.Combine(_fullPath, txtName), contents[i]);

            await worker.FromTextToTempFile();
        }

        var recordsResult = recordJsonFile.ReadFile(recordService.GetFileName());
        mockDateTimeProvider.Setup(m => m.Now).Returns(dates[8]);

        await worker.FromTempToArchiveFileAsync();

        var dailySetsResult = dailySetJsonFile.ReadFile(dailySetService.GetFileName());

        var tagResult = tagsJsonFile.ReadFile(tagService.GetFileName());
        var txtLenght = new FileInfo(txtPath).Length;
        var json24Lenght = new FileInfo(json24Path).Length;

        if (File.Exists(json24Path))
            File.Delete(json24Path);
        if (File.Exists(tagsPath))
            File.Delete(tagsPath);
        if (File.Exists(dailySetsPath))
            File.Delete(dailySetsPath);

        // Assert
        mockDailyRecordsSetEmail.Verify(m => m.SendEmailAsync(
            It.IsAny<DailyRecordsSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()), Times.Exactly(3));
        recordsResult.Should().BeEquivalentTo(expectedRecords);
        dailySetsResult.Should().BeEquivalentTo(expectedDailySets);
        tagResult.Should().BeEquivalentTo(expectedTags);
        txtLenght.Should().Be(0);
        json24Lenght.Should().Be(0);
    }
    public static TheoryData<DateTimeOffset[], string[]> FromTempToArchiveFileAsync_Test2 =>
       new TheoryData<DateTimeOffset[], string[]>
       {
            {DateArray, contentArrayWithTags},
       };

    [Theory]
    [MemberData(nameof(FromTempToArchiveFileAsync_Test2))]
    public async Task FromTextToTempFile_FromTempToArchiveFileAsync_Test2(DateTimeOffset[] dates, string[] contents)
    {
        // Arrange
        string txtName = "notes.txt";
        string json24Name = "Records_from_last_24_h.json";
        string tagsName = "Tags_list.json";
        string dailySetsName = "RecordBook_lipiec_2023.json";

        var mockLogger = new Mock<ILogger<Worker>>();
        var mockDailyRecordsSetManager = new Mock<IDailyRecordsSetManager>();
        var mockTimerConfig = new Mock<ITimerConfig>();
        var mockConfig = new Mock<IConfiguration>();

        var mockDailyRecordsSetEmail = new Mock<IEmailService<DailyRecordsSet>>();
        mockDailyRecordsSetEmail.Setup(m => m.SendEmailAsync(It.IsAny<DailyRecordsSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()))
               .ReturnsAsync(1);
        var mockRecordEmail = new Mock<IEmailService<Domain.Entity.Record>>();

        var mockDateTimeProvider = new Mock<IDateTimeProvider>();

        var mockFileConfig = new Mock<IFileConfig>();
        mockFileConfig.Setup(m => m.JsonFileLocation).Returns(_fullPath);
        mockFileConfig.Setup(m => m.TxtFileLocation).Returns(_fullPath);
        mockFileConfig.Setup(m => m.TxtName).Returns(txtName);

        // Real instances
        var tagsJsonFile = new JsonFileService<Tag>(mockFileConfig.Object);
        var txtFile = new TxtFileService(mockFileConfig.Object);
        var recordJsonFile = new JsonFileService<Domain.Entity.Record>(mockFileConfig.Object);
        var dailySetJsonFile = new JsonFileService<DailyRecordsSet>(mockFileConfig.Object);
        var recordService = new RecordService();

        var tagService = new TagService();
        var dailySetService = new DailyRecordsSetService();

        var dailySetManager = new DailyRecordsSetManager(mockDateTimeProvider.Object, dailySetJsonFile, dailySetService);
        var tagManager = new TagManager(mockDateTimeProvider.Object, tagsJsonFile, tagService);
        var recordManager = new RecordManager(mockDateTimeProvider.Object, txtFile, recordJsonFile, recordService);

        var worker = new Worker(mockLogger.Object,
                                tagManager,
                                recordManager,
                                mockDateTimeProvider.Object,
                                dailySetManager,
                                mockFileConfig.Object,
                                mockTimerConfig.Object,
                                mockDailyRecordsSetEmail.Object,
                                mockRecordEmail.Object,
                                mockConfig.Object);

        var expectedDailySets = new List<DailyRecordsSet> { GetDailyRecordsSetsArray2[0], GetDailyRecordsSetsArray2[1], GetDailyRecordsSetsArray2[2] };

        var expectedRecords = GetRecorsList;
       
        var expectedTags = new List<Tag> { GetTagsArray2[0], GetTagsArray[1], GetTagsArray[2] };

        var txtPath = Path.Combine(_fullPath, txtName);
        var json24Path = Path.Combine(_fullPath, json24Name);
        var tagsPath = Path.Combine(_fullPath, tagsName);
        var dailySetsPath = Path.Combine(_fullPath, dailySetsName);

        // Act
        for (int i = 0; i < 4; i++)
        {
            mockDateTimeProvider.Setup(m => m.DateOfContent).Returns(dates[i * 2]);
            mockDateTimeProvider.Setup(m => m.Now).Returns(dates[2 * i + 1]);
            File.AppendAllText(Path.Combine(_fullPath, txtName), contents[i]);

            await worker.FromTextToTempFile();
            await worker.FromTempToArchiveFileAsync();
        }

        var dailySetsResult = dailySetJsonFile.ReadFile(dailySetService.GetFileName());

        var tagResult = tagsJsonFile.ReadFile(tagService.GetFileName());
        var txtLenght = new FileInfo(txtPath).Length;
        var json24Lenght = new FileInfo(json24Path).Length;

        if (File.Exists(json24Path))
            File.Delete(json24Path);
        if (File.Exists(tagsPath))
            File.Delete(tagsPath);
        if (File.Exists(dailySetsPath))
            File.Delete(dailySetsPath);

        // Assert
        mockDailyRecordsSetEmail.Verify(m => m.SendEmailAsync(
            It.IsAny<DailyRecordsSet>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken?>()), Times.Exactly(4));
        dailySetsResult.Should().BeEquivalentTo(expectedDailySets);
        tagResult.Should().BeEquivalentTo(expectedTags);
        txtLenght.Should().Be(0);
        json24Lenght.Should().Be(0);
    }

}