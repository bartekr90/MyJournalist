using MyJournalist.App.Abstract;
using MyJournalist.App.Concrete;
using MyJournalist.App.Managers;
using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Tests.Managers;

public class DailyRecordsSetManagerTests
{
    private readonly DailyRecordsSetManager _sut;
    private readonly Mock<IFileCollectionService<DailyRecordsSet>> _fileServiceMock = new Mock<IFileCollectionService<DailyRecordsSet>>();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new Mock<IDateTimeProvider>();
    private readonly IDailyRecordsSetService _dailySetService = new DailyRecordsSetService();

    public DailyRecordsSetManagerTests()
    {
        _sut = new DailyRecordsSetManager(_dateTimeProviderMock.Object, _fileServiceMock.Object, _dailySetService);
    }

    [Fact]
    public void GetDailyRecordsSet_ShouldReturnDailyRecordsSet_WhenRecordsAndTagsAreProvided()
    {
        // Arrange
        var records = new List<Domain.Entity.Record> { new Domain.Entity.Record() };
        var tags = new List<Tag> { new Tag() };
        var expectedDailyRecordsSet = new DailyRecordsSet();
        var serviceMock = new Mock<IDailyRecordsSetService>();
        var manager = new DailyRecordsSetManager(_dateTimeProviderMock.Object, _fileServiceMock.Object, serviceMock.Object);

        serviceMock.Setup(x => x.GetDailyRecordsSet(records, tags, It.IsAny<IDateTimeProvider>())).Returns(expectedDailyRecordsSet);

        // Act
        var result = manager.GetDailyRecordsSet(records, tags);

        // Assert
        result.Should().BeSameAs(expectedDailyRecordsSet);
    }

    [Fact]
    public void GetDailyRecordsSet_ShouldReturnEmptyDailyRecordsSet_WhenNoRecordsProvided()
    {
        // Arrange
        ICollection<Domain.Entity.Record>? records = null;
        ICollection<Tag>? tags = null;
        var expectedEmptyDailyRecordsSet = new DailyRecordsSet();
        var serviceMock = new Mock<IDailyRecordsSetService>();
        var manager = new DailyRecordsSetManager(_dateTimeProviderMock.Object, _fileServiceMock.Object, serviceMock.Object);

        serviceMock.Setup(x => x.GetEmptyObj(It.IsAny<IDateTimeProvider>())).Returns(expectedEmptyDailyRecordsSet);

        // Act
        var result = manager.GetDailyRecordsSet(records, tags);

        // Assert
        result.Should().BeSameAs(expectedEmptyDailyRecordsSet);
    }

    public static TheoryData<List<Domain.Entity.Record>, Tuple<DateTimeOffset, DateTimeOffset>, List<Tag>> ShouldReturnDailyRecordsSet_WhenRecordsIsNull =>
        new TheoryData<List<Domain.Entity.Record>, Tuple<DateTimeOffset, DateTimeOffset>, List<Tag>>
        {
            {null, tupleArray[14], new List<Tag>() },
            {new List<Domain.Entity.Record>(), tupleArray[17], TagsMerged_Nr05_Nr08() },
            {new List<Domain.Entity.Record>(), tupleArray[17], null },
            {null, tupleArray[17], null },
        };
    [Theory]
    [MemberData(nameof(ShouldReturnDailyRecordsSet_WhenRecordsIsNull))]
    public void GetDailyRecordsSet_ShouldReturnDailyRecordsSet_WhenRecordsIsNull(List<Domain.Entity.Record> records,
                                                                            Tuple<DateTimeOffset, DateTimeOffset> tuple,
                                                                            List<Tag> tags)
    {
        // Arrange
        var refersToDate = tuple.Item1;
        var dateNow = tuple.Item2;
        var provider = _dateTimeProviderMock.Object;

        _dateTimeProviderMock.Setup(dp => dp.Now).Returns(dateNow);
        _dateTimeProviderMock.Setup(dp => dp.DateOfContent).Returns(refersToDate);
        _sut.SetDateForService(refersToDate);

        // Act
        var result = _sut.GetDailyRecordsSet(records, tags);

        // Assert
        result.Should().NotBeNull();
        result.HasAnyRecords.Should().BeFalse();
        result.Records.Should().BeNullOrEmpty();
        result.CreatedDateTime.Should().Be(provider.Now);
        result.ModifiedDateTime.Should().Be(provider.Now);
        result.RefersToDate.Should().Be(provider.DateOfContent);
        result.EmailHasBeenSent.Should().BeFalse();
        result.HasAnyTags.Should().BeFalse();
        result.MergedTags.Should().BeNullOrEmpty();
        result.MergedContents.Should().Be("no content");
        result.MergedContents.Should().NotBeNullOrWhiteSpace();
    }

    public static TheoryData<List<Domain.Entity.Record>, string, Tuple<DateTimeOffset, DateTimeOffset>, List<Tag>, bool> GetDailyRecordsSet_ShouldReturnDailyRecordsSet =>
      new TheoryData<List<Domain.Entity.Record>, string, Tuple<DateTimeOffset, DateTimeOffset>, List<Tag>, bool>
      {
            {RecordsListNr3Empty(), "no content", tupleArray[14], new List<Tag>(), false },
            {RecordsListNr3Empty(), "no content", tupleArray[14], TagsMerged_Nr05_Nr08(), true },
            {RecordListMarch2023nr1(), ContentMergedMarch2023nr1, tupleArray[13], null, false },
            {RecordListMarch2023nr2(), ContentMergedMarch2023nr2, tupleArray[17], new List<Tag>(), false },
            {RecordListMarch2023nr3(), ContentMergedMarch2023nr3, tupleArray[21], TagsMerged_Nr05_Nr08(), true },
            {RecordListMarch2023nr4(), ContentMergedMarch2023nr4, tupleArray[25], TagsMerged_Nr06_Nr07(), true }
      };

    [Theory]
    [MemberData(nameof(GetDailyRecordsSet_ShouldReturnDailyRecordsSet))]
    public void GetDailyRecordsSet_ShouldReturnDailyRecordsSet_WhenRecordsNotNull(List<Domain.Entity.Record> records,
                                                                                  string expectedMergedContent,
                                                                                  Tuple<DateTimeOffset, DateTimeOffset> tuple,
                                                                                  List<Tag> tags,
                                                                                  bool hasTags)
    {
        // Arrange
        var refersToDate = tuple.Item1;
        var dateNow = tuple.Item2;
        var provider = _dateTimeProviderMock.Object;

        _dateTimeProviderMock.Setup(dp => dp.Now).Returns(dateNow);
        _dateTimeProviderMock.Setup(dp => dp.DateOfContent).Returns(refersToDate);
        _sut.SetDateForService(refersToDate);

        // Act
        var result = _sut.GetDailyRecordsSet(records, tags);

        // Assert
        result.Should().NotBeNull();
        result.Records.Should().BeEquivalentTo(records);
        result.HasAnyRecords.Should().BeTrue();
        result.CreatedDateTime.Should().Be(provider.Now);
        result.ModifiedDateTime.Should().Be(provider.Now);
        result.RefersToDate.Should().Be(provider.DateOfContent);
        result.EmailHasBeenSent.Should().BeFalse();
        result.HasAnyTags.Should().Be(hasTags);
        result.MergedTags.Should().BeEquivalentTo(tags);
        result.MergedContents.Should().Be(expectedMergedContent);
        result.MergedContents.Should().NotBeNullOrWhiteSpace();
    }


    [Fact]
    public void SaveListInFile_ShouldDoNothing_WhenSetsFromMonthIsEmpty()
    {
        // Arrange
        var setsFromMonth = new List<DailyRecordsSet>();

        // Act
        _sut.SaveListInFile(setsFromMonth);

        // Assert
        _fileServiceMock.Verify(f => f.ClearFile(It.IsAny<string>()), Times.Never);
        _fileServiceMock.Verify(f => f.UpdateFile(It.IsAny<ICollection<DailyRecordsSet>>(), It.IsAny<string>()), Times.Never);
        _dailySetService.RefersToDate.Should().Be(new DateTimeOffset());
    }

    [Fact]
    public void SaveListInFile_ShouldLoadDailySetsFromFile_WhenSetsFromMonthIsNotEmpty()
    {
        // Arrange
        var setsFromMonth = new List<DailyRecordsSet>
            {
                new DailyRecordsSet { RefersToDate = tupleArray[33].Item1 },
                new DailyRecordsSet { RefersToDate = tupleArray[34].Item1 }
            };
        List<DailyRecordsSet> setsFromFile = DailyRecordsSetsListAprilNr2();
     

        _fileServiceMock.Setup(f => f.ReadFile(It.IsAny<string>())).Returns(setsFromFile);
        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(true); 

        // Act
        _sut.SaveListInFile(setsFromMonth);

        // Assert
        _dailySetService.RefersToDate.Should().Be(tupleArray[33].Item1);
        _dailySetService.GetAllItems().Should().BeEquivalentTo(setsFromFile);
        _fileServiceMock.Verify(f => f.ReadFile(It.IsAny<string>()), Times.Once);
        _fileServiceMock.Verify(f => f.ClearFile(It.IsAny<string>()), Times.Once);
        _fileServiceMock.Verify(f => f.UpdateFile(It.IsAny<List<DailyRecordsSet>>(), It.IsAny<string>()), Times.Once);
        _fileServiceMock.Verify(f => f.CreateFile(It.IsAny<string>()), Times.Never);
    }

    public static TheoryData<List<DailyRecordsSet>, List<DailyRecordsSet>, List<DailyRecordsSet>> SaveListInFile_SampleData =>
         new TheoryData<List<DailyRecordsSet>, List<DailyRecordsSet>, List<DailyRecordsSet>>
         {
             {DailyRecordsSetsListAprilNr2Id_2_3, DailyRecordsSetsListAprilNr2Id_4_5, DailyRecordsSetsListAprilNr2()},
             {DailyRecordsSetsListAprilNr2Id_2_4, DailyRecordsSetsListAprilNr2().Where(r => r.Id == 5).ToList(), DailyRecordsSetsListAprilNr2()},
         };

    [Theory]
    [MemberData(nameof(SaveListInFile_SampleData))]
    public void SaveListInFile_ShouldMakeUnionOfDailySets_WhenSetsFromMonthIsNotEmpty(List<DailyRecordsSet> setsFromFile,
                                                                                      List<DailyRecordsSet> setsFromMonth,
                                                                                      List<DailyRecordsSet> expectedUnion)
    {
        // Arrange       
        List<DailyRecordsSet> setsToSave = null;         

        _fileServiceMock.Setup(f => f.ReadFile(It.IsAny<string>())).Returns(setsFromFile);
        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(true);

        _fileServiceMock.Setup(f => f.UpdateFile(It.IsAny<ICollection<DailyRecordsSet>>(), It.IsAny<string>()))
            .Callback<ICollection<DailyRecordsSet>, string>((s, f) => setsToSave = s.ToList()); 
        
        // Act
        _sut.SaveListInFile(setsFromMonth);

        // Assert
        setsToSave.Should().BeEquivalentTo(expectedUnion);
        ReferenceEquals(setsToSave, expectedUnion).Should().BeFalse();
        ReferenceEquals(setsToSave, _dailySetService.GetAllItems()).Should().BeFalse();
        ReferenceEquals(setsToSave, _dailySetService.Items).Should().BeFalse();
        _dailySetService.GetAllItems().Should().NotBeNull();

    }

    [Theory]
    [MemberData(nameof(SaveListInFile_SampleData))]
    public void SaveListInFile_ShouldUpdateFileWithsetsFromMonth_WhenFileDontExist(List<DailyRecordsSet> setsFromFile,
                                                                                   List<DailyRecordsSet> setsFromMonth,
                                                                                   List<DailyRecordsSet> expectedUnion)
    {
        // Arrange
        expectedUnion = setsFromMonth;
        List<DailyRecordsSet> setsToSave = null; 
        string fileName = "SomeFile";

        _fileServiceMock.Setup(f => f.ReadFile(fileName)).Returns(setsFromFile);
        _fileServiceMock.Setup(f => f.CheckFileExists(fileName)).Returns(true);

        _fileServiceMock.Setup(f => f.UpdateFile(It.IsAny<ICollection<DailyRecordsSet>>(), It.IsAny<string>()))
            .Callback<ICollection<DailyRecordsSet>, string>((s, f) => setsToSave = s.ToList()); 
        
        // Act
        _sut.SaveListInFile(setsFromMonth);

        // Assert
        setsToSave.Should().BeEquivalentTo(expectedUnion);
        _dailySetService.GetAllItems().Should().BeNullOrEmpty();
        _fileServiceMock.Verify(f => f.CreateFile(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void SetDateForService_ShouldSetRefersToDate_WhenCalled()
    {
        // Arrange
        var date = new DateTimeOffset(2021, 10, 1, 0, 0, 0, TimeSpan.Zero);

        // Act
        _sut.SetDateForService(date);

        // Assert
        _dailySetService.RefersToDate.Should().Be(date);
    }
   
    [Fact]
    public void LoadDailySetsFromFile_ShouldReadFileAndSetItems_WhenFileExists()
    {
        // Arrange
        var date = new DateTimeOffset(2021, 10, 1, 0, 0, 0, TimeSpan.Zero);
        var list = new List<DailyRecordsSet>
            {
                new DailyRecordsSet { RefersToDate = date },
                new DailyRecordsSet { RefersToDate = date.AddDays(1) }
            };
        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(true);
        _fileServiceMock.Setup(f => f.ReadFile(It.IsAny<string>())).Returns(list);

        // Act
        _sut.LoadDailySetsFromFile(date);

        // Assert
        _dailySetService.RefersToDate.Should().Be(date);
        _fileServiceMock.Verify(f => f.ReadFile(It.IsAny<string>()), Times.Once);
        _dailySetService.GetAllItems().Should().BeEquivalentTo(list);
    }

    [Fact]
    public void LoadDailySetsFromFile_ShouldCreateFile_WhenFileDoesNotExist()
    {
        // Arrange
        var date = new DateTimeOffset(2021, 11, 1, 0, 0, 0, TimeSpan.Zero);
        string name = "";
        _fileServiceMock.Setup(f => f.CheckFileExists(It.IsAny<string>())).Returns(false)
            .Callback<string>((s) => name = s);        

        // Act
        _sut.LoadDailySetsFromFile(date);

        // Assert
        _fileServiceMock.Verify(f => f.CreateFile(name), Times.Once);
        _dailySetService.RefersToDate.Should().Be(date);
        _fileServiceMock.Verify(f => f.ReadFile(It.IsAny<string>()), Times.Never);
        name.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GroupDailySetsByMonth_ShouldReturnEmptyDictionary_WhenDailySetsIsEmpty()
    {
        // Arrange
        var dailySets = new List<DailyRecordsSet>();

        // Act
        var result = _sut.GroupDailySetsByMonth(dailySets);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GroupDailySetsByMonth_ShouldGroupDailySetsByYearAndMonth_WhenDailySetsIsNotEmpty()
    {
        // Arrange
        var dailySets = new List<DailyRecordsSet>
    {
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 1)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 2)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 2, 1)) }
    };

        // Act
        var result = _sut.GroupDailySetsByMonth(dailySets);

        // Assert
        result.Should().HaveCount(2);
        result[(2022, 1)].Should().HaveCount(2);
        result[(2022, 2)].Should().HaveCount(1);
    }
    
    [Fact]
    public void GroupDailySetsByMonth_ShouldReturnEmptyDictionary_WhenDailySetsIsNull()
    {
        // Arrange
        List<DailyRecordsSet> dailySets = null;

        //Act
        var result = _sut.GroupDailySetsByMonth(dailySets);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GroupDailySetsByMonth_ShouldReturnDictionaryWithSingleKey_WhenAllDailySetsHaveSameYearAndMonth()
    {
        // Arrange
        var dailySets = new List<DailyRecordsSet>
    {
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 1)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 2)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 3)) }
    };

        // Act
        var result = _sut.GroupDailySetsByMonth(dailySets);

        // Assert
        result.Should().HaveCount(1);
        result[(2022, 1)].Should().HaveCount(3);
    }

    [Fact]
    public void GroupDailySetsByMonth_ShouldReturnCorrectlyGroupedDictionary_WhenDailySetsIsNotEmpty()
    {
        // Arrange
        var dailySets = new List<DailyRecordsSet>
    {
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 1)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 2)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 2, 1)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2023, 1, 1)) }
    };

        // Act
        var result = _sut.GroupDailySetsByMonth(dailySets);

        // Assert
        result.Should().HaveCount(3);
        result[(2022, 1)].Should().HaveCount(2);
        result[(2022, 2)].Should().HaveCount(1);
        result[(2023, 1)].Should().HaveCount(1);
    }

    [Fact]
    public void GroupDailySetsByMonth_ShouldReturnDictionaryWithMultipleKeys_WhenDailySetsHaveDifferentYearsAndMonths()
    {
        // Arrange
        var dailySets = new List<DailyRecordsSet>
    {
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 1, 1)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2022, 2, 1)) },
        new DailyRecordsSet { RefersToDate = new DateTimeOffset(new DateTime(2023, 1, 1)) }
    };

        // Act
        var result = _sut.GroupDailySetsByMonth(dailySets);

        // Assert
        result.Should().HaveCount(3);
        result[(2022, 1)].Should().HaveCount(1);
        result[(2022, 2)].Should().HaveCount(1);
        result[(2023, 1)].Should().HaveCount(1);
    } 
    
    [Fact]
    public void GroupDailySetsByMonth_ShouldReturnDictionaryWithMultipleKeys_Test2()
    {
        // Arrange     
        List<DailyRecordsSet> concatSets = DailyRecordsSetsListNr1().Concat(DailyRecordsSetsMarch2023Nr1()).Concat(DailyRecordsSetsListAprilNr2()).ToList();

        // Act
        var result = _sut.GroupDailySetsByMonth(concatSets);

        // Assert
        result.Should().HaveCount(3);
        result[(2023, 4)].Should().HaveCount(4);
        result[(2023, 3)].Should().HaveCount(5);
        result[(2023, 5)].Should().HaveCount(4);
        result.Should().NotBeEmpty();
        result.Keys.Should().HaveCount(3);
        result.Values.Should().OnlyContain(v => v.Count > 0);
    }

}
