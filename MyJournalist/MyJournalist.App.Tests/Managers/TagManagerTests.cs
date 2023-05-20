using Moq;
using MyJournalist.App.Abstract;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        ICollection<Tag>? primaryTags = TagsMergedMarch2023TokensNr5();
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
        var expectedTags = new List<Tag> { new Tag { Id = 1}, new Tag { Id = 1} };
        _tagServiceMock.Setup(t => t.FindTokens(content)).Returns(2);
        _tagServiceMock.Setup(t => t.FindAllTags(content, _dateTimeProviderMock.Object, 2)).Returns(expectedTags);

        // Act
        var tags = _sut.GetTagsFromContent(content);

        // Assert
        _tagServiceMock.Verify(f => f.FindTokens(content), Times.Once);
        _tagServiceMock.Verify(f => f.FindAllTags(content, _dateTimeProviderMock.Object, 2), Times.Once);
        tags.Should().BeEquivalentTo(expectedTags);        
    }

    //[Fact]
    //public void MergeTagsFromRecordList_ShouldMergeTagsFromRecords_WhenRecordsNotNull()
    //{
    //    // Arrange
    //    var records = new List<Record>
    //    {
    //        new Record { Tags = new List<Tag> { new Tag("Tag1"), new Tag("Tag2") } },
    //        new Record { Tags = new List<Tag> { new Tag("Tag2"), new Tag("Tag3") } }
    //    };
    //    var expectedMergedTags = new List<Tag> { new Tag("Tag1"), new Tag("Tag2"), new Tag("Tag3") };

    //    // Act
    //    var mergedTags = _sut.MergeTagsFromRecordList(records);

    //    // Assert
    //    mergedTags.Should().BeEquivalentTo(expectedMergedTags);
    //}
}
