using MyJournalist.App.Abstract;
using MyJournalist.App.Concrete;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Tests.Concrete;

public class TagServiceTests
{
    private readonly TagService _sut;
    private readonly Mock<IDateTimeProvider> _dTProviderMock = new Mock<IDateTimeProvider>();

    public TagServiceTests()
    {
        _sut = new TagService();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("Some text without tags")]
    [InlineData("This is a sentence without any tags")]
    public void FindAllTags_ShouldReturnNull_WhenTagsNotFound(string content)
    {
        // Act
        List<Tag> tags = _sut.FindAllTags(content, _dTProviderMock.Object, 0);

        // Assert
        tags.Should().BeNull();
    }

    public static TheoryData<string, string[]> ShouldReturnTags_WhenTagsFound =>
        new TheoryData<string, string[]>
        {
            { "Some text with #TAG1 and #tagI2", new string[] {"tag1", "tagi2"} },
            { MergedContentArrayWithTagsId0_3, subTagsArrayId0_3 },
            { MergedContentArrayWithTagsId4_7, subTagsArrayId4_7 },
            { MergedContentArrayWithTagsId8_11, subTagsArrayId8_11 }
        };

    [Theory]
    [MemberData(nameof(ShouldReturnTags_WhenTagsFound))]
    public void FindAllTags_ShouldReturnTags_WhenTagsFound(string content, string[] tags)
    {
        // Arrange

        // Act
        List<Tag> resultTags = _sut.FindAllTags(content, _dTProviderMock.Object, 0);

        // Assert
        resultTags.Should().NotBeNull();
        resultTags.Should().HaveCount(tags.Length);
        resultTags.Select(t => t.Name).Should().Contain(tags);
    }

    public static TheoryData<string, Tuple<DateTimeOffset, DateTimeOffset>, uint, List<Tag>> ShouldReturnTags_ForGivenData =>
        new TheoryData<string, Tuple<DateTimeOffset, DateTimeOffset>, uint, List<Tag>>
        {
            {MergedContentArrayWithTagsId0_3, tupleArray[29], 3, TagsMergedMarch2023TokensNr5()},
            {MergedContentArrayWithTagsId4_7, tupleArray[33], 0, TagsMerged01April2023Nr6()},
            {MergedContentArrayWithTagsId8_11, tupleArray[37], 0, TagsMerged02April2023Nr7()},
        };

    [Theory]
    [MemberData(nameof(ShouldReturnTags_ForGivenData))]
    public void FindAllTags_ShouldReturnTags_ForGivenData(string content, Tuple<DateTimeOffset, DateTimeOffset> tuple, uint tokens, List<Tag> tags)
    {
        // Arrange
        _dTProviderMock.Setup(dp => dp.DateOfContent).Returns(tuple.Item1);
        _dTProviderMock.Setup(dp => dp.Now).Returns(tuple.Item2);
        var providerMock = _dTProviderMock.Object;

        var expectedTags = new List<Tag>();
        foreach (var tag in tags)
        {
            tag.Id = 0;
            tag.IdOfInitialRecord = 0;
            tag.CreatedById = 0;
            tag.ModifiedById = null;
            tag.NameOfInitialRecord = "";
            tag.ContentDate = tuple.Item1;
            tag.CreatedDateTime = tuple.Item2;
            tag.ModifiedDateTime = tuple.Item2;

            expectedTags.Add(tag);
        }

        // Act
        List<Tag> resultTags = _sut.FindAllTags(content, providerMock, tokens);

        // Assert
        resultTags.Should().BeEquivalentTo(expectedTags);
    }

    public static TheoryData<string> ShouldReturnZero_ForExampleData =>
        new TheoryData<string>
        {
            null,
            string.Empty,
            "",
            "   ",
            contentArrayWithTags[5],
            contentArrayWithTags[6],
            contentArrayWithTags[7]
        };

    [Theory]
    [MemberData(nameof(ShouldReturnZero_ForExampleData))]
    public void FindTokens_ShouldReturnZero_ForExampleData(string content)
    {
        // Arrange

        // Act
        uint result = _sut.FindTokens(content);

        // Assert
        result.Should().Be(0);
    }

    public static TheoryData<string, uint> ShouldReturnTokenValue_WhenContentContainsValidData =>
      new TheoryData<string, uint>
      {
          { "Lorem ipsum dolor sit $123 amet", 123 },
          { "Lorem ipsum dolor sit $ 123 amet", 0 },
          { "Lorem ipsum dolor sit $(123) amet", 0 },
          { "Lorem ipsum dolor sit $xz123 amet", 0 },
          { "Lorem ipsum dolor sit $xz(123) amet", 0 },
          { "Lorem ipsum dolor sit $123abc amet", 123 },
          { "Lorem ipsum $8,8 dolor sit $-123abc amet", 8 },
          { "Lorem ipsum $-9.2 dolor sit $123abc amet", 123 },
          { "$sda123abc amet", 0 },
          { "$sda 123 abc amet", 0 },
          { "$-4 abc amet", 0 },
          { "$-4 abc $3amet", 3 },
          { "$4.5 abc amet", 4 },
          { "$(4) abc amet", 0 },
          { "$ 4) abc amet", 0 },
          {contentArrayWithTags[0], 3},
          {contentArrayWithTags[1], 3},
          {contentArrayWithTags[2], 3},
          {contentArrayWithTags[4], 12},
          {ContentMerged03April2023TagsNr8, 5}
      };

    [Theory]
    [MemberData(nameof(ShouldReturnTokenValue_WhenContentContainsValidData))]
    public void FindTokens_ShouldReturnTokenValue_WhenContentContainsValidData(string content, uint example)
    {
        // Arrange

        // Act
        uint result = _sut.FindTokens(content);

        // Assert
        result.Should().Be(example);
    }

    [Fact]
    public void MakeUnion_ShouldReturnNull_WhenBothParametersAreNull()
    {
        // Arrange
        ICollection<Tag>? primaryTags = null;
        ICollection<Tag>? newTags = null;

        // Act
        var result = _sut.MakeUnion(primaryTags, newTags);

        // Assert
        result.Should().BeNull();
    }

    public static TheoryData<List<Tag>, List<Tag>, List<Tag>> ShouldReturnNewTags_ForExampleData =>
        new TheoryData<List<Tag>, List<Tag>, List<Tag>>
        {
            {TagsMerged01April2023Nr6(), null, TagsMerged01April2023Nr6() },
            {null, TagsMerged01April2023Nr6(), TagsMerged01April2023Nr6() },
            {TagsMerged01April2023Nr6(), TagsMerged01April2023Nr6(), TagsMerged01April2023Nr6() },
            {TagsMerged01April2023Nr6(), TagsMerged02April2023Nr7(), TagsMerged_Nr06_Nr07()},
            {TagsMergedMarch2023TokensNr5(), TagsMerged03April2023Nr8(), TagsMerged_Nr05_Nr08()}
        };

    [Theory]
    [MemberData(nameof(ShouldReturnNewTags_ForExampleData))]
    public void MakeUnion_ShouldReturnNewTags_ForExampleData(ICollection<Tag> tags1, ICollection<Tag> tags2, ICollection<Tag> expectedTags)
    {
        // Arrange

        // Act
        List<Tag> result = _sut.MakeUnion(tags1, tags2);

        // Assert
        result.Should().BeEquivalentTo(expectedTags);
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenNamesAreEqualIgnoringCase()
    {
        // Arrange
        var tag1 = new Tag { Name = "tag" };
        var tag2 = new Tag { Name = "Tag" };

        // Act
        var result = _sut.Equals(tag1, tag2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenNamesAreNotEqualIgnoringCase()
    {
        // Arrange
        var tag1 = new Tag { Name = "tag1" };
        var tag2 = new Tag { Name = "tag2" };

        // Act
        var result = _sut.Equals(tag1, tag2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldCombineTimeTokens_WhenNamesAreEqualIgnoringCase()
    {
        // Arrange
        var tag1 = new Tag { Name = "tag" };
        var tag2 = new Tag { Name = "Tag", TimeTokens = 5 };

        // Act
        var result = _sut.Equals(tag1, tag2);

        // Assert
        result.Should().BeTrue();
        tag1.TimeTokens.Should().Be(5);
    }
    [Fact]
    public void GetEmptyObj_ShouldReturnTagWithModifiedDateTimeAndCreatedDateTimeSetToProviderNow()
    {
        // Arrange
        var now = new DateTime(2023, 5, 19, 10, 0, 0);
        _dTProviderMock.Setup(p => p.Now).Returns(now);

        // Act
        var result = _sut.GetEmptyObj(_dTProviderMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ModifiedDateTime.Should().Be(now);
        result.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void GetEmptyObj_ShouldReturnTagWithContentDateSetToProviderDateOfContent()
    {
        // Arrange
        var dateOfContent = new DateTime(2023, 5, 19);
        _dTProviderMock.Setup(p => p.DateOfContent).Returns(dateOfContent);

        // Act
        var result = _sut.GetEmptyObj(_dTProviderMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.ContentDate.Should().Be(dateOfContent);
    }

    [Fact]
    public void GetFileName_ShouldReturnFileName()
    {
        // Act
        var result = _sut.GetFileName();

        // Assert
        result.Should().Be(@"Tags_list");
    }

    [Fact]
    public void GetTag_ShouldReturnTagWithNameAndTimeTokensSet()
    {
        // Arrange
        var name = "tag";
        var tokens = 10u;
        var expectedTag = new Tag { Name = name, TimeTokens = tokens, ModifiedDateTime = DateTimeOffset.MinValue };

        // Act
        var result = _sut.GetTag(name, _dTProviderMock.Object, tokens);

        // Assert
        result.Should().BeEquivalentTo(expectedTag);
    }

    [Fact]
    public void GetHashCode_ShouldReturnHashCodeOfLowercaseTagName()
    {
        // Arrange
        var tagName = "Tag";
        var tag = new Tag { Name = tagName };

        // Act
        var result = _sut.GetHashCode(tag);

        // Assert
        result.Should().Be(tagName.ToLower().GetHashCode());
    }

    [Fact]
    public void GetTag_ShouldSetTagNameAndTimeTokens()
    {
        // Arrange
        var name = "TestTag";
        var tokens = 10u;
        var now = new DateTime(2023, 5, 19, 10, 0, 0);
        var dateOfContent = new DateTime(2023, 5, 19);

        _dTProviderMock.Setup(p => p.Now).Returns(now);
        _dTProviderMock.Setup(p => p.DateOfContent).Returns(dateOfContent);


        // Act
        var result = _sut.GetTag(name, _dTProviderMock.Object, tokens);

        // Assert
        result.Name.Should().Be(name);
        result.TimeTokens.Should().Be(tokens);
        result.ContentDate.Should().Be(dateOfContent);
        result.ModifiedDateTime.Should().Be(now);
        result.CreatedDateTime.Should().Be(now);
    }

    [Fact]
    public void GetTag_ShouldReturnTagWithDefaultName_WhenNameIsNull()
    {
        // Arrange
        string name = null;
        var tokens = 10u;

        // Act
        var tag = _sut.GetTag(name, _dTProviderMock.Object, tokens);

        // Assert
        tag.Name.Should().Be("DefaultName");
        tag.TimeTokens.Should().Be(tokens);
    }

    [Theory]
    [MemberData(nameof(ShouldReturnTokenValue_WhenContentContainsValidData))]
    public void GetTag_ShouldReturnTagAndTimeTokens_WhenFindTokensCalls(string content, uint expectedTokens)
    {
        // Arrange
        uint tokens = _sut.FindTokens(content);

        // Act
        var tag = _sut.GetTag(content, _dTProviderMock.Object, tokens);

        // Assert
        tag.Name.Should().Be(content);
        tokens.Should().Be(expectedTokens);
        tag.TimeTokens.Should().Be(tokens);
    }
}
