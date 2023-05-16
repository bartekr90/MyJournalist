using MyJournalist.App.Abstract;
using MyJournalist.App.Concrete;
using MyJournalist.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
           {new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero), TagsMergedMarch2023TokensNr5(), 3, "some content", true},
           {new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), TagsMergedMarch2023TokensNr5(), 3, " ", false},
           { new DateTimeOffset(2023, 5, 7, 10, 30, 0, 123, TimeSpan.Zero), TagsMerged01April2023Nr6(), 0, "", false },
           { new DateTimeOffset(2023, 5, 7, 0, 0, 0, TimeSpan.Zero), TagsMerged02April2023Nr7(), 0, "someContent", true },
           { new DateTimeOffset(2023, 5, 7, 23, 59, 59, TimeSpan.Zero), TagsMerged03April2023Nr8(), 0, null, false },
           { new DateTimeOffset(2013, 5, 7, 0, 0, 0, TimeSpan.Zero), TagsMerged04April2023Nr9(), 0, "      ", false },
           { new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), new List<Tag>(), 10, "sss", true },       
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
        _dTProviderMock.Setup(dp => dp.Now).Returns(date.AddMinutes(-10));
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(date);
                 
        // Act
        var result = _sut.GetRecordFromContent(content, tags, _dTProviderMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace();
        result.Content.Should().Be(content);
        result.ContentDate.Should().Be(date);
        result.HasContent.Should().Be(hasContent);
        result.HasAnyTags.Should().BeTrue();
        result.Tags.Should().BeEquivalentTo(tags);
        result.TimeTokens.Should().Be(tokens);
        result.CreatedDateTime.Should().Be(_dTProviderMock.Object.Now);
        result.ModifiedDateTime.Should().Be(_dTProviderMock.Object.Now);
    }

    //GetRecordFromContent_ShouldChangeTagsProp_WhenDateContentAndTagsIsSet

    //[Fact]
    //public void GetRecordFromContent_ShouldCreateRecordWithCorrectProperties_WhenContentAndTagsProvided()
    //{
    //    // Arrange
    //    var content = "Sample content";
    //    var tags = new List<Tag> { new Tag { TimeTokens = 5 } };
    //    var dateNow = new DateTimeOffset(2023, 5, 16, 10, 30, 0, TimeSpan.Zero);

    //    _dTProviderMock.Setup(dp => dp.Now).Returns(dateNow);
    //    _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(dateNow.AddDays(-1));

    //    var expectedName = "Rec_2023/05/16_10-30-00";
    //    var expectedTokens = 5;

    //    // Act
    //    var result = _sut.GetRecordFromContent(content, tags, _dTProviderMock.Object);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.Name.Should().Be(expectedName);
    //    result.Content.Should().Be(content);
    //    result.ContentDate.Should().Be(_dTProviderMock.Object.DateOfContent);
    //    result.HasContent.Should().BeTrue();
    //    result.HasAnyTags.Should().BeTrue();
    //    result.Tags.Should().BeEquivalentTo(tags);
    //    result.TimeTokens.Should().Be(expectedTokens);
    //    result.CreatedDateTime.Should().Be(_dTProviderMock.Object.Now);
    //    result.ModifiedDateTime.Should().Be(_dTProviderMock.Object.Now);
    //}

    //[Fact]
    //public void GetRecordFromContent_ShouldCreateRecordWithEmptyTags_WhenTagsIsNull()
    //{
    //    // Arrange
    //    var content = "Sample content";
    //    ICollection<Tag> tags = null;
    //    var provider = new MockDateTimeProvider(new DateTimeOffset(2023, 5, 16, 10, 30, 0, TimeSpan.Zero));

    //    // Act
    //    var result = GetRecordFromContent(content, tags, provider);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.HasAnyTags.Should().BeFalse();
    //    result.Tags.Should().BeEmpty();
    //}

    //[Fact]
    //public void GetRecordFromContent_ShouldCreateRecordWithEmptyTags_WhenTagsIsEmpty()
    //{
    //    // Arrange
    //    var content = "Sample content";
    //    var tags = new List<Tag>();
    //    var provider = new MockDateTimeProvider(new DateTimeOffset(2023, 5, 16, 10, 30, 0, TimeSpan.Zero));

    //    // Act
    //    var result = GetRecordFromContent(content, tags, provider);

    //    // Assert
    //    result.Should().NotBeNull();
    //    result.HasAnyTags.Should().BeFalse();
    //    result.Tags.Should().BeEmpty();
    //}
}
