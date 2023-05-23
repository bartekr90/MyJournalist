using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;
using Xunit;

namespace MyJournalist.App.Tests.Managers;

public class RecordManagerTests
{
    private readonly RecordManager _sut;
    private readonly Mock<IFileCollectionService<Domain.Entity.Record>> _fileServiceMock = new Mock<IFileCollectionService<Domain.Entity.Record>>();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new Mock<IDateTimeProvider>();
    private readonly Mock<IRecordService> _recordServiceMock = new Mock<IRecordService>();
    private readonly Mock<ITxtFileService> _txtFileServiceMock = new Mock<ITxtFileService>();

    public RecordManagerTests()
    {
        _sut = new RecordManager(_dateTimeProviderMock.Object, _txtFileServiceMock.Object, _fileServiceMock.Object, _recordServiceMock.Object);
    }

    [Fact]
    public void GetDataFromTxt_ShouldSetDateOfContent_WhenCalled()
    {
        // Arrange
        var lastWriteTime = new DateTime(2023, 5, 20);
        _txtFileServiceMock.Setup(x => x.GetLastWriteTime()).Returns(lastWriteTime);

        // Act
        var result = _sut.GetDataFromTxt();

        // Assert
        _dateTimeProviderMock.VerifySet(x => x.DateOfContent = lastWriteTime, Times.Once);
    }

    [Fact]
    public void GetDataFromTxt_ShouldReadDataFromTxtService_WhenCalled()
    {
        // Arrange
        var expectedData = "Test data";
        _txtFileServiceMock.Setup(x => x.ReadData()).Returns(expectedData);

        // Act
        var result = _sut.GetDataFromTxt();

        // Assert
        result.Should().Be(expectedData);
    }

    [Fact]
    public void GetDataFromTxt_ShouldReturnNull_WhenTxtServiceReturnsEmpty()
    {
        // Arrange
        _txtFileServiceMock.Setup(x => x.ReadData()).Returns("");

        // Act
        var result = _sut.GetDataFromTxt();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void ClearTxt_ShouldDoNothing_WhenDataClearingFails()
    {
        // Arrange
        _txtFileServiceMock.Setup(txt => txt.ClearData()).Throws(new Exception("Data clearing failed"));

        // Act
        Action act = () => _sut.ClearTxt();

        // Assert
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void ClearTxt_ShouldClearData()
    {
        // Arrange

        // Act
        _sut.ClearTxt();

        // Assert
        _txtFileServiceMock.Verify(txt => txt.ClearData(), Times.Once);
        _txtFileServiceMock.VerifyNoOtherCalls();
    }

    [Fact]
    public void FindPastDateRecords_ShouldReturnEmptyList_WhenListIsEmpty()
    {
        // Arrange
        var compareDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero);

        // Act
        var result = _sut.FindPastDateRecords(new List<Domain.Entity.Record>(), compareDate);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FindPastDateRecords_ShouldReturnRecordsWithDifferentDate_WhenCompareDateIsProvided()
    {
        // Arrange
        var compareDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero);
        var records = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 19, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 21, 0, 0, 0, TimeSpan.Zero) }
        };

        // Act
        var result = _sut.FindPastDateRecords(records, compareDate);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(records[0]);
        result.Should().Contain(records[2]);
        result.Should().NotContain(records[1]);
        result.Should().OnlyContain(record => record.ContentDate.Date != compareDate.Date);

    }

    [Fact]
    public void FindPastDateRecords_ShouldReturnEmptyList_WhenAllRecordsHaveSameDateAsCompareDate()
    {
        // Arrange
        var compareDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero);
        var records = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) }
        };

        // Act
        var result = _sut.FindPastDateRecords(records, compareDate);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FindPastDateRecords_ShouldReturnAllRecords_WhenCompareDateIsNew()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 19, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 21, 0, 0, 0, TimeSpan.Zero) }
        };
        var compareDate = new DateTimeOffset();
        // Act
        var result = _sut.FindPastDateRecords(records, compareDate);

        // Assert
        result.Should().HaveCount(3);
        result.Should().BeEquivalentTo(records);
        result.Should().OnlyContain(record => record.ContentDate.Date != compareDate.Date);

    }

    public static TheoryData<List<Domain.Entity.Record>, DateTimeOffset, List<Domain.Entity.Record>> FindPastDateRecords_ForSampleData =>
       new TheoryData<List<Domain.Entity.Record>, DateTimeOffset, List<Domain.Entity.Record>>
       {
            {CombinedListsFromNr1ToNr5, tupleArray[13].Item1, CombinedListsFromNr2ToNr5 },
            {CombinedListsFromNr6ToNr9, tupleArray[33].Item1, CombinedListsFromNr7ToNr9 }
       };

    [Theory]
    [MemberData(nameof(FindPastDateRecords_ForSampleData))]
    public void FindPastDateRecords_ShouldReturnRecordsList_ForSampleData(List<Domain.Entity.Record> records, DateTimeOffset compareDate, List<Domain.Entity.Record> example)
    {
        // Arrange       

        // Act
        List<Domain.Entity.Record> result = _sut.FindPastDateRecords(records, compareDate);

        // Assert
        result.Should().BeEquivalentTo(example);
        result.Should().OnlyContain(record => record.ContentDate.Date != compareDate.Date);
    }

    [Fact]
    public void FindEqualDateRecords_ShouldReturnEmptyList_WhenListIsNull()
    {
        // Arrange
        var compareDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero);
        var records = new List<Domain.Entity.Record>();
        // Act
        var result = _sut.FindEqualDateRecords(records, compareDate);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void FindEqualDateRecords_ShouldReturnRecordsWithSameDate_WhenCompareDateIsProvided()
    {
        // Arrange
        var compareDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero);
        var records = new List<Domain.Entity.Record>
    {
        new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 19, 0, 0, 0, TimeSpan.Zero) },
        new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
        new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 21, 0, 0, 0, TimeSpan.Zero) }
    };

        // Act
        var result = _sut.FindEqualDateRecords(records, compareDate);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(records[1]);
        result.Should().NotContain(records[0]);
        result.Should().NotContain(records[2]);
        result.Should().OnlyContain(record => record.ContentDate.Date == compareDate.Date);
    }

    [Fact]
    public void FindEqualDateRecords_ShouldReturnAllRecords_WhenCompareDateIsNew()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>
    {
        new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 19, 0, 0, 0, TimeSpan.Zero) },
        new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
        new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 21, 0, 0, 0, TimeSpan.Zero) }
    };
        var compareDate = new DateTimeOffset();

        // Act
        var result = _sut.FindEqualDateRecords(records, compareDate);

        // Assert
        result.Should().BeEmpty();
    }

    public static TheoryData<List<Domain.Entity.Record>, DateTimeOffset, List<Domain.Entity.Record>> FindEqualDateRecords_ForSampleData =>
       new TheoryData<List<Domain.Entity.Record>, DateTimeOffset, List<Domain.Entity.Record>>
       {
            {CombinedListsFromNr1ToNr5, tupleArray[13].Item1, RecordListMarch2023nr1() },
            {CombinedListsFromNr6ToNr9, tupleArray[33].Item1, RecordList01April2023TagsNr6() }
       };

    [Theory]
    [MemberData(nameof(FindEqualDateRecords_ForSampleData))]
    public void FindEqualDateRecords_ShouldReturnRecordsList_ForSampleData(List<Domain.Entity.Record> records, DateTimeOffset compareDate, List<Domain.Entity.Record> example)
    {
        // Arrange       

        // Act
        List<Domain.Entity.Record> result = _sut.FindEqualDateRecords(records, compareDate);

        // Assert
        result.Should().BeEquivalentTo(example);
        result.Should().OnlyContain(record => record.ContentDate.Date == compareDate.Date);
    }

    [Fact]
    public void GetRecordsWithContent_ShouldReturnEmptyList_WhenGroupIsNull()
    {
        // Arrange
        List<Domain.Entity.Record> group = new List<Domain.Entity.Record>();

        // Act
        var result = _sut.GetRecordsWithContent(group);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetRecordsWithContent_ShouldReturnRecordsWithContent_WhenGroupContainsRecordsWithContent()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { HasContent = true },
            new Domain.Entity.Record { HasContent = false },
            new Domain.Entity.Record { HasContent = true }
        };

        // Act
        var result = _sut.GetRecordsWithContent(records);

        // Assert
        result.Should().HaveCount(2);
        result.Should().OnlyContain(record => record.HasContent);
    }

    [Fact]
    public void GetRecordsWithContent_ShouldReturnEmptyList_WhenGroupContainsNoRecordsWithContent()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { HasContent = false },
            new Domain.Entity.Record { HasContent = false },
            new Domain.Entity.Record { HasContent = false }
        };

        // Act
        var result = _sut.GetRecordsWithContent(records);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetRecordsWithContent_ShouldReturnEmptyList_WhenGroupIsEmpty()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>();

        // Act
        var result = _sut.GetRecordsWithContent(records);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GroupRecordsByDate_ShouldReturnEmptyGroup_WhenRecordsIsNull()
    {
        // Arrange
        ICollection<Domain.Entity.Record> records = new List<Domain.Entity.Record>();

        // Act
        var result = _sut.GroupRecordsByDate(records);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GroupRecordsByDate_ShouldGroupRecordsByDate_WhenRecordsAreProvided()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 21, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 20, 0, 0, 0, TimeSpan.Zero) },
            new Domain.Entity.Record { ContentDate = new DateTimeOffset(2023, 5, 22, 0, 0, 0, TimeSpan.Zero) }
        };

        // Act
        var result = _sut.GroupRecordsByDate(records);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(group => group.Key == new DateTime(2023, 5, 20));
        result.Should().Contain(group => group.Key == new DateTime(2023, 5, 21));
        result.Should().Contain(group => group.Key == new DateTime(2023, 5, 22));
        result.Should().OnlyContain(group => group.All(record => record.ContentDate.Date == group.Key));
    }

    [Fact]
    public void GroupRecordsByDate_ShouldReturnEmptyGroup_WhenRecordsIsEmpty()
    {
        // Arrange
        var records = new List<Domain.Entity.Record>();

        // Act
        var result = _sut.GroupRecordsByDate(records);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GroupRecordsByDate_ShouldReturnRecordsGroupedByDate()
    {
        // Arrange       
        List<Domain.Entity.Record> records = CombinedListsFromNr6ToNr9;

        // Act
        IEnumerable<IGrouping<DateTime, Domain.Entity.Record>> result = _sut.GroupRecordsByDate(records);

        // Assert
        result.Should().HaveCount(4);
        result.Should().Contain(group => group.Key == tupleArray[33].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[37].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[41].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[45].Item1.Date);
        result.Should().OnlyContain(group => group.All(record => record.ContentDate.Date == group.Key));
    }

    [Fact]
    public void GroupRecordsByDate_ShouldReturnRecordsGroupedByDate_Test2()
    {
        // Arrange       
        List<Domain.Entity.Record> records = CombinedListsFromNr1ToNr5;

        // Act
        IEnumerable<IGrouping<DateTime, Domain.Entity.Record>> result = _sut.GroupRecordsByDate(records);

        // Assert
        result.Should().HaveCount(5);
        result.Should().Contain(group => group.Key == tupleArray[13].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[17].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[21].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[25].Item1.Date);
        result.Should().Contain(group => group.Key == tupleArray[29].Item1.Date);
        result.Should().OnlyContain(group => group.All(record => record.ContentDate.Date == group.Key));
    }

    [Fact]
    public void SaveRecordInFile_ShouldCallUpdateFileAndClearFile()
    {
        // Arrange
        var fileMock = _fileServiceMock.Object;
        var recordMock = _recordServiceMock.Object;
        var record = RecordListMarch2023nr1().First();
        var recordsFromFile = RecordListMarch2023nr1();

        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(true);
        _fileServiceMock.Setup(f => f.ReadFile(It.IsAny<string>())).Returns(recordsFromFile);
        _recordServiceMock.Setup(r => r.GetAllItems()).Returns(recordsFromFile);
        // Act
        _sut.SaveRecordInFile(record);

        // Assert
        _fileServiceMock.Verify(f => f.UpdateFile(It.IsAny<ICollection<Domain.Entity.Record>>(), recordMock.GetFileName()), Times.Once);
        _fileServiceMock.Verify(f => f.ClearFile(recordMock.GetFileName()), Times.Once);
    }

    [Fact]
    public void SaveRecordInFile_ShouldNotAddRecord_WhenRecordIsNull()
    {
        // Arrange
        var recordsFromFile = RecordListMarch2023nr1();
        Domain.Entity.Record record = null;

        // Act
        _sut.SaveRecordInFile(record);

        // Assert
        _recordServiceMock.Verify(s => s.GetAllItems(), Times.Never); 
        _fileServiceMock.Verify(s => s.ClearFile(It.IsAny<string>()), Times.Never); 
        _fileServiceMock.Verify(s => s.UpdateFile(It.IsAny<List<Domain.Entity.Record>>(), It.IsAny<string>()), Times.Never); 
    }

    [Fact]
    public void SaveListInFile_ShouldClearFileAndUpdateFile_WhenListIsNotEmpty()
    {
        // Arrange
        var fileName = "Records_from_last_24_h";
        var listToSave = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { Name = "Record1" },
            new Domain.Entity.Record { Name = "Record2" }
        };
        _recordServiceMock.Setup(s => s.GetFileName()).Returns(fileName);

        // Act
        _sut.SaveListInFile(listToSave);

        // Assert
        _fileServiceMock.Verify(s => s.ClearFile(fileName), Times.Once); 
        _fileServiceMock.Verify(s => s.UpdateFile(listToSave, fileName), Times.Once); 
    }

    [Fact]
    public void SaveListInFile_ShouldNotClearFileAndUpdateFile_WhenListIsEmpty()
    {
        // Arrange
        var listToSave = new List<Domain.Entity.Record>();

        // Act
        _sut.SaveListInFile(listToSave);

        // Assert
        _fileServiceMock.Verify(s => s.ClearFile(It.IsAny<string>()), Times.Never); 
        _fileServiceMock.Verify(s => s.UpdateFile(It.IsAny<List<Domain.Entity.Record>>(), It.IsAny<string>()), Times.Never); 
    }

    [Fact]
    public void SaveListInFile_ShouldNotClearFileAndUpdateFile_WhenListIsNull()
    {
        // Arrange
        List<Domain.Entity.Record> listToSave = null;

        // Act
        _sut.SaveListInFile(listToSave);

        // Assert
        _fileServiceMock.Verify(s => s.ClearFile(It.IsAny<string>()), Times.Never); 
        _fileServiceMock.Verify(s => s.UpdateFile(It.IsAny<List<Domain.Entity.Record>>(), It.IsAny<string>()), Times.Never); 
    }

    [Fact]
    public void LoadRecordsFromFile_ShouldSetItems_WhenFileExists()
    {
        // Arrange
        var recordsFromFile = new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record { Name = "Record1" },
            new Domain.Entity.Record { Name = "Record2" }
        };
        _fileServiceMock.Setup(s => s.CheckFileExists(It.IsAny<string>())).Returns(true);
        _fileServiceMock.Setup(s => s.ReadFile(It.IsAny<string>())).Returns(recordsFromFile);

        // Act
        _sut.LoadRecordsFromFile();

        // Assert
        _fileServiceMock.Verify(s => s.CheckFileExists(It.IsAny<string>()), Times.Once); 
        _fileServiceMock.Verify(s => s.ReadFile(It.IsAny<string>()), Times.Once); 
        _recordServiceMock.Verify(s => s.SetItems(recordsFromFile), Times.Once); 
        _fileServiceMock.Verify(s => s.CreateFile(It.IsAny<string>()), Times.Never); 
    }

    [Fact]
    public void LoadRecordsFromFile_ShouldSetTagsFromFile_WhenFileExists()
    {
        // Arrange
        var recordsFromFile = RecordListMarch2023nr1();

        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(true);
        _fileServiceMock.Setup(f => f.ReadFile(It.IsAny<string>())).Returns(recordsFromFile);

        // Act
        _sut.LoadRecordsFromFile();

        // Assert
        _recordServiceMock.Verify(t => t.SetItems(recordsFromFile), Times.Once);
        _recordServiceMock.Verify(t => t.GetFileName(), Times.Exactly(2));
        _fileServiceMock.Verify(f => f.ReadFile(It.IsAny<string>()), Times.Once);
        _fileServiceMock.Verify(f => f.CreateFile(It.IsAny<string>()), Times.Never);
    }
    [Fact]
    public void LoadRecordsFromFile_ShouldCreateFile_WhenFileDoesNotExist()
    {
        // Arrange
        _fileServiceMock.Setup(s => s.CheckFileExists(It.IsAny<string>())).Returns(false);

        // Act
        _sut.LoadRecordsFromFile();

        // Assert
        _fileServiceMock.Verify(s => s.ReadFile(It.IsAny<string>()), Times.Never); 
        _fileServiceMock.Verify(s => s.CheckFileExists(It.IsAny<string>()), Times.Once); 
        _recordServiceMock.Verify(s => s.SetItems(It.IsAny<List<Domain.Entity.Record>>()), Times.Never); 
        _fileServiceMock.Verify(s => s.CreateFile(It.IsAny<string>()), Times.Once);       
    }

    //[Fact]
    //public void GetRecord_ShouldReturnEmptyObj_WhenContentFromTxtIsNullOrWhiteSpace()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    string contentFromTxt = null;
    //    var tags = new List<Tag> { /* tags */ };
    //    var expectedRecord = new Record();
    //    _recordService.GetEmptyObj(_dateTimeProvider).Returns(expectedRecord);

    //    // Act
    //    var result = recordManager.GetRecord(contentFromTxt, tags);

    //    // Assert
    //    result.Should().Be(expectedRecord);
    //}

    //[Fact]
    //public void GetRecord_ShouldReturnRecordFromContent_WhenContentFromTxtIsValid()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var contentFromTxt = "Sample content";
    //    var tags = new List<Tag> { /* tags */ };
    //    var expectedRecord = new Record();
    //    _recordService.GetRecordFromContent(contentFromTxt, tags, _dateTimeProvider).Returns(expectedRecord);

    //    // Act
    //    var result = recordManager.GetRecord(contentFromTxt, tags);

    //    // Assert
    //    result.Should().Be(expectedRecord);
    //}


    //[Fact]
    //public void GetRecord_ShouldReturnEmptyRecord_WhenContentFromTxtIsNullOrWhiteSpace()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var tags = new List<Tag>();

    //    // Act
    //    var result = recordManager.GetRecord(null, tags);

    //    // Assert
    //    result.Should().BeEquivalentTo(_recordService.GetEmptyObj(_dateTimeProvider));
    //}

    //[Fact]
    //public void GetRecord_ShouldReturnRecordFromContent_WhenContentFromTxtIsNotNullOrWhiteSpace()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var contentFromTxt = "Sample content";
    //    var tags = new List<Tag>();

    //    // Act
    //    var result = recordManager.GetRecord(contentFromTxt, tags);

    //    // Assert
    //    result.Should().BeEquivalentTo(_recordService.GetRecordFromContent(contentFromTxt, tags, _dateTimeProvider));
    //}
    //[Fact]
    //public void MakeNewRecordList_ShouldAddRecordToList()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var record = new Record();
    //    var initialList = new List<Record> { /* initial records */ };
    //    _recordService.GetAllItems().Returns(initialList);

    //    // Act
    //    var result = recordManager.MakeNewRecordList(record);

    //    // Assert
    //    result.Should().Contain(record);
    //    result.Should().HaveCount(initialList.Count + 1);
    //}

    //[Fact]
    //public void SaveListInFile_ShouldClearAndSaveListToFile_WhenListHasItems()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var listToSave = new List<Record> { /* records to save */ };

    //    // Act
    //    recordManager.SaveListInFile(listToSave);

    //    // Assert
    //    _fileService.Received(1).ClearFile(_recordService.GetFileName());
    //    _fileService.Received(1).UpdateFile(listToSave, _recordService.GetFileName());
    //}

    //[Fact]
    //public void SaveListInFile_ShouldNotClearOrSaveListToFile_WhenListIsEmpty()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var listToSave = new List<Record>();

    //    // Act
    //    recordManager.SaveListInFile(listToSave);

    //    // Assert
    //    _fileService.DidNotReceive().ClearFile(_recordService.GetFileName());
    //    _fileService.DidNotReceive().UpdateFile(listToSave, _recordService.GetFileName());
    //}



    //[Fact]
    //public void ClearTxt_ShouldClearDataFromTxtService()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);

    //    // Act
    //    recordManager.ClearTxt();

    //    // Assert
    //    _txtService.Received(1).ClearData();
    //}



    //[Fact]
    //public void FindEqualDateRecords_ShouldReturnRecordsWithSameDate()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var list = new List<Record> { /* records */ };
    //    var compareDate = DateTimeOffset.Parse("2023-05-20");

    //    // Act
    //    var result = recordManager.FindEqualDateRecords(list, compareDate);

    //    // Assert
    //    result.Should().OnlyContain(record => record.ContentDate.Date == compareDate.Date);
    //}    






    //[Fact]
    //public void GetRecord_ShouldReturnEmptyObj_WhenContentFromTxtIsNullOrWhiteSpace()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    string contentFromTxt = null;
    //    var tags = new List<Tag> { /* tags */ };
    //    var expectedRecord = new Record();
    //    _recordService.GetEmptyObj(_dateTimeProvider).Returns(expectedRecord);

    //    // Act
    //    var result = recordManager.GetRecord(contentFromTxt, tags);

    //    // Assert
    //    result.Should().Be(expectedRecord);
    //}

    //[Fact]
    //public void GetRecord_ShouldReturnRecordFromContent_WhenContentFromTxtIsValid()
    //{
    //    // Arrange
    //    var recordManager = new RecordManager(_dateTimeProvider, _txtService, _fileService, _recordService);
    //    var contentFromTxt = "Sample content";
    //    var tags = new List<Tag> { /* tags */ };
    //    var expectedRecord = new Record();
    //    _recordService.GetRecordFromContent(contentFromTxt, tags, _dateTimeProvider).Returns(expectedRecord);

    //    // Act
    //    var result = recordManager.GetRecord(contentFromTxt, tags);

    //    // Assert
    //    result.Should().Be(expectedRecord);
    //}

}
