using MyJournalist.App.Abstract;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Tests.Managers;

public class TagManagerTests
{
    private readonly TagManager _sut;
    private readonly Mock<IFileCollectionService<Tag>> _fileServiceMock = new Mock<IFileCollectionService<Tag>>();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new Mock<IDateTimeProvider>();
    private readonly Mock<ITagService> _tagServiceMock = new Mock<ITagService>();

    public TagManagerTests()
    {
        _sut = new TagManager(_dateTimeProviderMock.Object, _fileServiceMock.Object, _tagServiceMock.Object);
    }

    [Fact]
    public void MargeAndSaveTagsInFile_ShouldCallUpdateFileAndClearFile()
    {
        // Arrange
        var fileMock = _fileServiceMock.Object;
        var tagMock = _tagServiceMock.Object;

        // Act
        _sut.MergeAndSaveTagsInFile(TagsMerged03April2023Nr8());

        // Assert
        _fileServiceMock.Verify(f => f.UpdateFile(It.IsAny<ICollection<Tag>>(), tagMock.GetFileName()), Times.Once);
        _fileServiceMock.Verify(f => f.ClearFile(tagMock.GetFileName()), Times.Once);
    }

    [Fact]
    public void MergeTags_ShouldCallMakeUnion_WhenBothInputTagsAreNull()
    {
        // Arrange
        ICollection<Tag>? primaryTags = null;
        ICollection<Tag>? newTags = null;

        _tagServiceMock.Setup(t => t.MakeUnion(primaryTags, newTags));

        // Act
        List<Tag> result = _sut.MergeTags(primaryTags, newTags);

        // Assert
        _tagServiceMock.Verify(x => x.MakeUnion(primaryTags, newTags), Times.Once);
        result.Should().BeNull();
    }

    [Fact]
    public void MergeTags_ShouldCallMakeUnion_WhenBothInputTagsNotNull()
    {
        // Arrange
        ICollection<Tag>? primaryTags = TagsMarch2023TokensNr5();
        ICollection<Tag>? newTags = TagsMerged03April2023Nr8();
        var expected = TagsMerged_Nr05_Nr08();

        _tagServiceMock.Setup(t => t.MakeUnion(primaryTags, newTags)).Returns(expected);

        // Act
        List<Tag> result = _sut.MergeTags(primaryTags, newTags);

        // Assert
        _tagServiceMock.Verify(x => x.MakeUnion(primaryTags, newTags), Times.Once);
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void LoadTagsFromFile_ShouldSetTagsFromFile_WhenFileExists()
    {
        // Arrange
        var tagsFromFile = TagsMerged_Nr05_Nr08();

        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(true);
        _fileServiceMock.Setup(f => f.ReadFile(It.IsAny<string>())).Returns(tagsFromFile);

        // Act
        _sut.LoadTagsFromFile();

        // Assert
        _tagServiceMock.Verify(t => t.SetItems(tagsFromFile), Times.Once);
        _tagServiceMock.Verify(t => t.GetFileName(), Times.Exactly(2));
        _fileServiceMock.Verify(f => f.ReadFile(It.IsAny<string>()), Times.Once);
        _fileServiceMock.Verify(f => f.CreateFile(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void LoadTagsFromFile_ShouldCreateFile_WhenFileDoesNotExist()
    {
        // Arrange
        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(false);

        // Act
        _sut.LoadTagsFromFile();

        // Assert
        _fileServiceMock.Verify(f => f.CreateFile(It.IsAny<string>()), Times.Once);
        _tagServiceMock.Verify(t => t.SetItems(It.IsAny<List<Tag>>()), Times.Never);
    }

    [Fact]
    public void GetTagsFromContent_ShouldReturnTagsFromContent()
    {
        // Arrange
        var content = "Some content with #Tag1 and #Tag2";
        var expectedTags = new List<Tag> { new Tag { Id = 1 }, new Tag { Id = 1 } };
        _tagServiceMock.Setup(t => t.FindTokens(content)).Returns(2);
        _tagServiceMock.Setup(t => t.FindAllTags(content, _dateTimeProviderMock.Object, 2)).Returns(expectedTags);

        // Act
        var tags = _sut.GetTagsFromContent(content);

        // Assert
        _tagServiceMock.Verify(f => f.FindTokens(content), Times.Once);
        _tagServiceMock.Verify(f => f.FindAllTags(content, _dateTimeProviderMock.Object, 2), Times.Once);
        tags.Should().BeEquivalentTo(expectedTags);
    }

    [Fact]
    public void MergeTagsFromRecordList_ShouldReturnEmptyList_WhenRecordsIsNull()
    {
        // Arrange
        List<Domain.Entity.Record> records = null;

        // Act
        List<Tag> result = _sut.MergeTagsFromRecordList(records);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void MergeTagsFromRecordList_ShouldReturnEmptyList_WhenRecordsIsEmpty()
    {
        // Arrange
        List<Domain.Entity.Record> records = new List<Domain.Entity.Record>();

        // Act
        List<Tag> result = _sut.MergeTagsFromRecordList(records);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void MergeTagsFromRecordList_ShouldCallMakeUnionTwice_WhenMergeTagsFromAllRecordsCalled()
    {
        // Arrange
        List<Domain.Entity.Record> records = new List<Domain.Entity.Record>();

        var record1 = new Domain.Entity.Record
        {
            Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "Tag1"
                    },
                    new Tag
                    {
                        Name = "Tag2"
                    }
                }
        };
        records.Add(record1);

        var record2 = new Domain.Entity.Record
        {
            Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "Tag3"
                    },
                    new Tag
                    {
                        Name = "Tag4"
                    }
                }
        };
        records.Add(record2);

        _tagServiceMock.Setup(t => t.MakeUnion(It.IsAny<ICollection<Tag>>(), It.IsAny<ICollection<Tag>>()));

        // Act
        List<Tag> result = _sut.MergeTagsFromRecordList(records);

        // Assert
        _tagServiceMock.Verify(x => x.MakeUnion(It.IsAny<ICollection<Tag>>(), It.IsAny<ICollection<Tag>>()), Times.Exactly(2));
    }

    [Fact]
    public void MergeTagsFromRecordList_ShouldMergeTagsFromRecords_WhenRecordsNotNull()
    {
        // Arrange
        List<Domain.Entity.Record> records = new List<Domain.Entity.Record>();

        var record1 = new Domain.Entity.Record
        {
            Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "Tag1"
                    },
                    new Tag
                    {
                        Name = "Tag2"
                    }
                }
        };
        records.Add(record1);

        var record2 = new Domain.Entity.Record
        {
            Tags = new List<Tag>
                {
                    new Tag
                    {
                        Name = "Tag3"
                    },
                    new Tag
                    {
                        Name = "Tag4"
                    }
                }
        };
        records.Add(record2);
        var expectedMergedTags = new List<Tag> { new Tag { Name = "Tag1" }, new Tag { Name = "Tag2" }, new Tag { Name = "Tag3" }, new Tag { Name = "Tag4" } };

        TagService tagService = new TagService();
        TagManager tagManager = new TagManager(_dateTimeProviderMock.Object, _fileServiceMock.Object, tagService);

        // Act
        List<Tag> result = tagManager.MergeTagsFromRecordList(records);

        // Assert
        result.Should().BeEquivalentTo(expectedMergedTags);
    }

    public static TheoryData<List<Domain.Entity.Record>, List<Tag>> ShouldMergeTagsFromRecords_WhenRecordsNotNull =>
        new TheoryData<List<Domain.Entity.Record>, List<Tag>>
        {
            { RecordListMarch2023TagsTokensNr5(), TagsMergedMarch2023TokensNr5()},
            { RecordList01April2023TagsNr6(), TagsMerged01April2023Nr6()},
            { RecordList02April2023TagsNr7(), TagsMerged02April2023Nr7},
            { RecordList03April2023TagsNr8(), TagsMerged03April2023Nr8()},
            { RecordList04April2023TagsNr9(), TagsMerged04April2023Nr9},
        };

    [Theory]
    [MemberData(nameof(ShouldMergeTagsFromRecords_WhenRecordsNotNull))]
    public void MergeTagsFromRecordList_ShouldMergeTagsFromRecords_WhenRecordsNotNull_Test2(List<Domain.Entity.Record> records, List<Tag> expected)
    {
        // Arrange       
        TagService tagService = new TagService();
        TagManager tagManager = new TagManager(_dateTimeProviderMock.Object, _fileServiceMock.Object, tagService);

        // Act
        List<Tag> result = tagManager.MergeTagsFromRecordList(records);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
