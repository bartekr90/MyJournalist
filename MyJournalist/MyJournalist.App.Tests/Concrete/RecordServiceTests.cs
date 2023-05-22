using MyJournalist.App.Abstract;
using MyJournalist.App.Concrete;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Tests.Concrete;

public class RecordServiceTests
{
    private readonly RecordService _sut;
    private readonly Mock<IDateTimeProvider> _dTProviderMock = new Mock<IDateTimeProvider>();

    public RecordServiceTests()
    {
        _sut = new RecordService();
    }

    public static TheoryData<DateTimeOffset, string> ShouldCreateRecordName_WhenDateOfContentIsSet =>
       new TheoryData<DateTimeOffset, string>
       {
           { new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero), "Rec_07/01/2001_10-30" },
           { new DateTimeOffset(2023, 5, 7, 10, 30, 0, 123, TimeSpan.Zero), "Rec_07/05/2023_10-30" },
           { new DateTimeOffset(2023, 5, 7, 0, 0, 0, TimeSpan.Zero), "Rec_07/05/2023_00-00" },
           { new DateTimeOffset(2023, 5, 7, 23, 59, 59, TimeSpan.Zero), "Rec_07/05/2023_23-59" },
           { new DateTimeOffset(1, 1, 1, 0, 0, 0, TimeSpan.Zero), "Rec_01/01/0001_00-00" },
           { new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), "Rec_31/12/9999_23-59" },
           { new DateTimeOffset(2013, 5, 7, 10, 30, 0, TimeSpan.FromHours(-13)), "Rec_07/05/2013_10-30" },
           { new DateTimeOffset(2003, 4, 7, 10, 30, 0, TimeSpan.FromHours(5)), "Rec_07/04/2003_10-30" },
           { new DateTimeOffset(2023, 3, 7, 10, 30, 0, TimeSpan.FromHours(-5)), "Rec_07/03/2023_10-30" },
           { new DateTimeOffset(), "Rec_01/01/0001_00-00" },
           { DateTimeOffset.MaxValue, "Rec_31/12/9999_23-59" },
           { DateTimeOffset.MinValue, "Rec_01/01/0001_00-00" }
       };

    [Theory]
    [MemberData(nameof(ShouldCreateRecordName_WhenDateOfContentIsSet))]
    public void GetRecordFromContent_ShouldCreateRecordName_WhenDateOfContentIsSet(DateTimeOffset date, string name)
    {
        // Arrange
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(date);

        // Act
        var result = _sut.GetRecordFromContent("", null, _dTProviderMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.Content.Should().BeEmpty();
        result.ContentDate.Should().Be(date);
        result.HasContent.Should().BeFalse();
        result.HasAnyTags.Should().BeFalse();
        result.Tags.Should().BeNull();
        result.TimeTokens.Should().Be(0);
    }

    public static TheoryData<DateTimeOffset, List<Tag>, uint, string, bool> ShouldCreateRecord_WhenDateContentAndTagsIsSet =>
       new TheoryData<DateTimeOffset, List<Tag>, uint, string, bool>
       {
           {new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero), TagsMarch2023TokensNr5(), 3, "some content", true},
           {new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), TagsMarch2023TokensNr5(), 3, " ", false},
           { new DateTimeOffset(2023, 5, 7, 10, 30, 0, 123, TimeSpan.Zero), TagsMerged01April2023Nr6(), 0, "", false },
           { new DateTimeOffset(2023, 5, 7, 0, 0, 0, TimeSpan.Zero), Tags02April2023Nr7(), 0, "someContent", true },
           { new DateTimeOffset(2023, 5, 7, 23, 59, 59, TimeSpan.Zero), TagsMerged03April2023Nr8(), 5, null, false },
           { new DateTimeOffset(2013, 5, 7, 0, 0, 0, TimeSpan.Zero), Tags04April2023Nr9(), 0, "      ", false },
       };

    [Theory]
    [MemberData(nameof(ShouldCreateRecord_WhenDateContentAndTagsIsSet))]
    public void GetRecordFromContent_ShouldCreateRecord_WhenDateContentAndTagsIsSet(DateTimeOffset date,
                                                                                    List<Tag> tags,
                                                                                    uint tokens,
                                                                                    string content,
                                                                                    bool hasContent)
    {
        // Arrange
        _dTProviderMock.Setup(dp => dp.Now).Returns(date);
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(date.AddMinutes(-10));

        // Act
        var result = _sut.GetRecordFromContent(content, tags, _dTProviderMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace();
        result.Content.Should().Be(content);
        result.ContentDate.Should().Be(_dTProviderMock.Object.DateOfContent);
        result.HasContent.Should().Be(hasContent);
        result.HasAnyTags.Should().BeTrue();
        result.Tags.Should().BeEquivalentTo(tags);
        result.TimeTokens.Should().Be(tokens);
        result.CreatedDateTime.Should().Be(_dTProviderMock.Object.Now);
        result.ModifiedDateTime.Should().Be(_dTProviderMock.Object.Now);
    }

    [Fact]
    public void GetRecordFromContent_ShouldCreateRecord_WhenTagsAreEmpty()
    {
        //Arrange
        var date = new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero);
        string recName = "Rec_07/01/2001_10-20";
        List<Tag> tags = new List<Tag>();
        uint tokens = 0;
        string content = "Some exaple content";

        _dTProviderMock.Setup(dp => dp.Now).Returns(date);
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(date.AddMinutes(-10));

        // Act
        var result = _sut.GetRecordFromContent(content, tags, _dTProviderMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(recName);
        result.Content.Should().Be(content);
        result.ContentDate.Should().Be(_dTProviderMock.Object.DateOfContent);
        result.HasContent.Should().Be(true);
        result.HasAnyTags.Should().BeFalse();
        result.Tags.Should().BeEquivalentTo(tags);
        result.TimeTokens.Should().Be(tokens);
        result.CreatedDateTime.Should().Be(_dTProviderMock.Object.Now);
        result.ModifiedDateTime.Should().Be(_dTProviderMock.Object.Now);
    }

    public static TheoryData<DateTimeOffset, List<Tag>, string> ShouldChangeTagsProp_WhenDateTimeProvIsSet =>
       new TheoryData<DateTimeOffset, List<Tag>, string>
       {
           {new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero), TagsMarch2023TokensNr5(), "Rec_07/01/2001_10-30"},
           {new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), TagsMarch2023TokensNr5(), "Rec_31/12/9999_23-59"},
           {new DateTimeOffset(2023, 5, 7, 10, 30, 0, 123, TimeSpan.Zero), TagsMerged01April2023Nr6(), "Rec_07/05/2023_10-30"},
           {new DateTimeOffset(2023, 5, 7, 0, 0, 0, TimeSpan.Zero), Tags02April2023Nr7(), "Rec_07/05/2023_00-00"},
           {new DateTimeOffset(2023, 5, 7, 23, 59, 59, TimeSpan.Zero), TagsMerged03April2023Nr8(), "Rec_07/05/2023_23-59" },
           {new DateTimeOffset(2013, 5, 7, 0, 0, 0, TimeSpan.Zero), Tags04April2023Nr9(), "Rec_07/05/2013_00-00"}
       };

    [Theory]
    [MemberData(nameof(ShouldChangeTagsProp_WhenDateTimeProvIsSet))]
    public void GetRecordFromContent_ShouldChangeTagsProp_WhenDateTimeProvIsSet(DateTimeOffset date, List<Tag> tags, string recName)
    {
        // Arrange
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(date);

        // Act
        var result = _sut.GetRecordFromContent("", tags, _dTProviderMock.Object);

        // Assert
        foreach (var tag in result.Tags)
        {
            tag.Should().NotBeNull();
            tag.Name.Should().NotBeNullOrEmpty();
            tag.NameOfInitialRecord.Should().Be(recName);
        }
    }

    [Fact]
    public void GetEmptyObj_ShouldReturnEmptyRecord()
    {
        // Arrange
        var date = new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero);

        _dTProviderMock.Setup(dp => dp.Now).Returns(date);
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(date.AddMinutes(-10));

        var expectedRecord = new Domain.Entity.Record
        {
            Id = 0,
            Name = "Rec_07/01/2001_10-20",
            ContentDate = _dTProviderMock.Object.DateOfContent,
            Content = "[no content]",
            HasAnyTags = false,
            HasContent = false,
            CreatedDateTime = _dTProviderMock.Object.Now,
            ModifiedDateTime = _dTProviderMock.Object.Now
        };

        // Act
        var record = _sut.GetEmptyObj(_dTProviderMock.Object);

        // Assert
        record.Should().NotBeNull();
        record.Should().BeEquivalentTo(expectedRecord);
        record.ContentDate.Should().Be(_dTProviderMock.Object.DateOfContent);
        record.CreatedDateTime.Should().Be(_dTProviderMock.Object.Now);
        record.ModifiedDateTime.Should().Be(_dTProviderMock.Object.Now);
    }

    [Fact]
    public void GetFileName_ShouldReturnCorrectFileName()
    {
        // Arrange

        // Act
        var fileName = _sut.GetFileName();

        // Assert
        fileName.Should().Be(@"Records_from_last_24_h");
    }

}
