using MyJournalist.App.Abstract;
using MyJournalist.App.Common;
using MyJournalist.App.Concrete;
using MyJournalist.Domain.Entity;


namespace MyJournalist.App.Tests.Concrete;

public class DailyRecordsSetServiceTests
{
    private readonly DailyRecordsSetService _sut;
    private readonly Mock<IDateTimeProvider> _dateTimeMock = new Mock<IDateTimeProvider>();

    public DailyRecordsSetServiceTests()
    {
        _sut = new DailyRecordsSetService();
    }

    public static TheoryData<DateTimeOffset, string> DataGetFileName =>
        new TheoryData<DateTimeOffset, string>
        {
            { new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero), "RecordBook_styczeń_2001" },
            { new DateTimeOffset(2023, 5, 7, 10, 30, 0, 123, TimeSpan.Zero), "RecordBook_maj_2023" },
            { new DateTimeOffset(2023, 5, 7, 0, 0, 0, TimeSpan.Zero), "RecordBook_maj_2023" },
            { new DateTimeOffset(2023, 5, 7, 23, 59, 59, TimeSpan.Zero), "RecordBook_maj_2023" },
            { new DateTimeOffset(1, 1, 1, 0, 0, 0, TimeSpan.Zero), "RecordBook_styczeń_1" },
            { new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero), "RecordBook_grudzień_9999" },
            { new DateTimeOffset(2013, 5, 7, 10, 30, 0, TimeSpan.FromHours(-7)), "RecordBook_maj_2013" },
            { new DateTimeOffset(2003, 4, 7, 10, 30, 0, TimeSpan.FromHours(5)), "RecordBook_kwiecień_2003" },
            { new DateTimeOffset(2023, 3, 7, 10, 30, 0, TimeSpan.FromHours(-5)), "RecordBook_marzec_2023" },
            { new  DateTimeOffset(), "RecordBook_styczeń_1" },
            { DateTimeOffset.MaxValue, "RecordBook_grudzień_9999" },
            { DateTimeOffset.MinValue, "RecordBook_styczeń_1" }
        };
    [Theory]
    [MemberData(nameof(DataGetFileName))]
    public void GetFileName_ShouldReturnFormattedFileName_WhenRefersToDateIsSet(DateTimeOffset date, string name)
    {
        // Arrange
        _sut.RefersToDate = date;

        // Act
        var fileName = _sut.GetFileName();

        // Assert
        fileName.Should().Be(name);
    }

    [Fact]
    public void GetFileName_ShouldReturnFormattedFileName_WhenRefersToDateIsNotSet()
    {
        // Arrange       

        // Act
        var fileName = _sut.GetFileName();

        // Assert
        fileName.Should().Be("RecordBook_styczeń_1");
    }

    public static TheoryData<Tuple<DateTimeOffset, DateTimeOffset>> DataForGetEmptyObj =>
        new TheoryData<Tuple<DateTimeOffset, DateTimeOffset>>
        {
            GetDateSetDiff0SecMax,
            GetDateSetDiff0SecMin,
            GetDateSetDiff1Day,
            GetDateSetDiff1Min,
            GetDateSetDiff2000Yers,
            GetDateSetDiff2Hours,
            GetDateSetDiff40Days,
            GetDateSetDiff8Hours,
            GetDateSetDiff9998Yers,
            GetDateSetDiff59Sec,
            GetDateSetDiff3Years,
            GetDateSetDiff89Days
        };

    [Theory]
    [MemberData(nameof(DataForGetEmptyObj))]
    public void GetEmptyObj_ShouldReturnEmptyDailyRecordsSet_WhenDateAndProviderIsSet(Tuple<DateTimeOffset, DateTimeOffset> tuple)
    {
        // Arrange 
        var refersToDate = tuple.Item1;
        var dateNow = tuple.Item2;

        _dateTimeMock.Setup(dp => dp.Now).Returns(dateNow);
        _sut.RefersToDate = refersToDate;

        DailyRecordsSet set = new DailyRecordsSet
        {
            CreatedDateTime = _dateTimeMock.Object.Now,
            ModifiedDateTime = _dateTimeMock.Object.Now,
            RefersToDate = refersToDate,
        };
        // Act
        DailyRecordsSet result = _sut.GetEmptyObj(_dateTimeMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DailyRecordsSet>();
        result.RefersToDate.Should().BeOnOrBefore(_dateTimeMock.Object.Now);
        result.RefersToDate.Should().Be(_sut.RefersToDate);
        set.CreatedDateTime.Should().Be(result.CreatedDateTime);
        set.ModifiedDateTime.Should().Be(result.ModifiedDateTime);
        set.RefersToDate.Should().Be(result.RefersToDate);
    }

    public static TheoryData<DateTimeOffset> DataGetEmptyObj =>
        new TheoryData<DateTimeOffset>
        {
             DateTimeOffset.Now,
             new DateTimeOffset(),
             DateTimeOffset.MaxValue,
             DateTimeOffset.MinValue,
             new DateTimeOffset(2023, 5, 9, 10, 0, 0, TimeSpan.Zero),
        };
    [Theory]
    [MemberData(nameof(DataGetEmptyObj))]
    public void GetEmptyObj_ShouldReturnEmptyDailyRecordsSet_WhenRefersToDateIsSet(DateTimeOffset refersToDate)
    {
        // Arrange 
        _sut.RefersToDate = refersToDate;

        // Act
        DailyRecordsSet emptyObj = _sut.GetEmptyObj(new DateTimeProvider());

        // Assert
        emptyObj.Should().BeOfType<DailyRecordsSet>();
        emptyObj.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(DataForGetEmptyObj))]
    public void GetEmptyObj_ShouldReturnObjWithCorrectedDate_WhenBadDataWasPassed(Tuple<DateTimeOffset, DateTimeOffset> tuple)
    {
        // Arrange 
        var refersToDate = tuple.Item2;
        var dateNow = tuple.Item1;

        _dateTimeMock.Setup(dp => dp.Now).Returns(dateNow);
        _sut.RefersToDate = refersToDate;

        DailyRecordsSet set = new DailyRecordsSet
        {
            CreatedDateTime = _dateTimeMock.Object.Now,
            ModifiedDateTime = _dateTimeMock.Object.Now,
            RefersToDate = _dateTimeMock.Object.Now,
        };
        // Act
        DailyRecordsSet emptyObj = _sut.GetEmptyObj(_dateTimeMock.Object);

        // Assert
        emptyObj.Should().NotBeNull();
        emptyObj.Should().BeOfType<DailyRecordsSet>();
        emptyObj.RefersToDate.Should().BeOnOrBefore(_dateTimeMock.Object.Now);
        set.CreatedDateTime.Should().Be(emptyObj.CreatedDateTime);
        set.ModifiedDateTime.Should().Be(emptyObj.ModifiedDateTime);
        set.RefersToDate.Date.Should().Be(emptyObj.RefersToDate.Date);
    }

    public static TheoryData<List<Domain.Entity.Record>, string, List<Tag>, Tuple<DateTimeOffset, DateTimeOffset>> DataGetDailyRecordsSet =>
        new TheoryData<List<Domain.Entity.Record>, string, List<Tag>, Tuple<DateTimeOffset, DateTimeOffset>>
        {
            {RecordsListNr1(), GetMergedContentNr1, GetTagListNr1(), GetDateSetDiff1Day },
            {RecordListMarch2023TagsTokensNr5(), ContentMergedMarch2023TagsTokensNr5, TagsMarch2023TokensNr5(), tupleArray[32]},
            {RecordList01April2023TagsNr6(), ContentMerged01April2023TagsNr6, TagsMerged01April2023Nr6(), tupleArray[33]},
            {RecordList02April2023TagsNr7(), ContentMerged02April2023TagsNr7, Tags02April2023Nr7(), tupleArray[37]},
            {RecordList03April2023TagsNr8(), ContentMerged03April2023TagsNr8, TagsMerged03April2023Nr8(), tupleArray[41]},
            {RecordList04April2023TagsNr9(), ContentMerged04April2023TagsNr9, Tags04April2023Nr9(), tupleArray[45]}
        };

    [Theory]
    [MemberData(nameof(DataGetDailyRecordsSet))]
    public void GetDailyRecordsSet_ShouldSetProperties_WhenRecordsAndTagsAreProvided(List<Domain.Entity.Record> records,
                                                                                     string mergedContent,
                                                                                     List<Tag> tags,
                                                                                     Tuple<DateTimeOffset, DateTimeOffset> tuple)
    {
        // Arrange
        var refersToDate = tuple.Item1;
        var dateNow = tuple.Item2;

        _sut.RefersToDate = refersToDate;
        _dateTimeMock.Setup(dp => dp.Now).Returns(dateNow);

        // Act
        var result = _sut.GetDailyRecordsSet(records, tags, _dateTimeMock.Object);

        // Assert
        result.Records.Should().BeEquivalentTo(records);
        result.HasAnyRecords.Should().BeTrue();
        result.EmailHasBeenSent.Should().BeFalse();
        result.Should().NotBeNull();
        result.Should().BeOfType<DailyRecordsSet>();
        result.RefersToDate.Should().BeOnOrBefore(_dateTimeMock.Object.Now);
        result.RefersToDate.Should().Be(_sut.RefersToDate);
        result.CreatedDateTime.Should().Be(dateNow);
        result.ModifiedDateTime.Should().Be(dateNow);
        result.MergedContents.Should().NotBeNullOrEmpty();
        result.MergedContents.Should().Be(mergedContent);
        result.MergedTags.Should().BeEquivalentTo(tags);
        result.HasAnyTags.Should().BeTrue();
    }

    public static TheoryData<List<Domain.Entity.Record>, string, Tuple<DateTimeOffset, DateTimeOffset>> DataNoTagsGetDailyRecordsSet =>
       new TheoryData<List<Domain.Entity.Record>, string, Tuple<DateTimeOffset, DateTimeOffset>>
       {
            {RecordsListNr2April(), GetMergedContentFromApril, GetDateSetDiff8Hours },
            {RecordsListNr3Empty(), "no content", tupleArray[14] },
            {RecordListMarch2023nr1(), ContentMergedMarch2023nr1, tupleArray[13] },
            {RecordListMarch2023nr2(), ContentMergedMarch2023nr2, tupleArray[17] },
            {RecordListMarch2023nr3(), ContentMergedMarch2023nr3, tupleArray[21] },
            {RecordListMarch2023nr4(), ContentMergedMarch2023nr4, tupleArray[25] }
       };

    [Theory]
    [MemberData(nameof(DataNoTagsGetDailyRecordsSet))]
    public void GetDailyRecordsSet_ShouldSetProperties_WhenRecordsAndNoTagsProvided(List<Domain.Entity.Record> records,
                                                                                    string mergedContent,
                                                                                    Tuple<DateTimeOffset, DateTimeOffset> tuple)
    {
        // Arrange
        var refersToDate = tuple.Item1;
        var dateNow = tuple.Item2;

        _sut.RefersToDate = refersToDate;
        _dateTimeMock.Setup(dp => dp.Now).Returns(dateNow);

        // Act
        var result = _sut.GetDailyRecordsSet(records, null, _dateTimeMock.Object);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<DailyRecordsSet>();
        result.Records.Should().BeEquivalentTo(records);
        result.HasAnyRecords.Should().BeTrue();
        result.MergedContents.Should().NotBeNullOrEmpty();
        result.MergedContents.Should().Be(mergedContent);
        result.CreatedDateTime.Should().Be(dateNow);
        result.ModifiedDateTime.Should().Be(dateNow);
        result.RefersToDate.Should().BeOnOrBefore(_dateTimeMock.Object.Now);
        result.RefersToDate.Should().Be(_sut.RefersToDate);
        result.EmailHasBeenSent.Should().BeFalse();
        result.MergedTags.Should().BeNullOrEmpty();
        result.HasAnyTags.Should().BeFalse();
    }

    [Fact]
    public void MakeUnion_ShouldReturnNull_WhenBothSetsAreNull()
    {
        // Arrange
        ICollection<DailyRecordsSet> primarySets = null;
        ICollection<DailyRecordsSet> newSets = null;

        // Act
        var result = _sut.MakeUnion(primarySets, newSets);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void MakeUnion_ShouldReturnNewSets_WhenPrimarySetsAreNull()
    {
        // Arrange
        ICollection<DailyRecordsSet> primarySets = null;
        ICollection<DailyRecordsSet> newSets = new List<DailyRecordsSet>();

        // Act
        var result = _sut.MakeUnion(primarySets, newSets);

        // Assert
        result.Should().BeEquivalentTo(newSets);
    }

    [Fact]
    public void MakeUnion_ShouldReturnPrimarySets_WhenNewSetsAreNull()
    {
        // Arrange
        ICollection<DailyRecordsSet> primarySets = new List<DailyRecordsSet>();
        ICollection<DailyRecordsSet> newSets = null;

        // Act
        var result = _sut.MakeUnion(primarySets, newSets);

        // Assert
        result.Should().BeEquivalentTo(primarySets);
    }

    public static TheoryData<List<DailyRecordsSet>, List<DailyRecordsSet>, List<DailyRecordsSet>> DataMakeUnionNotOverlaping =>
        new TheoryData<List<DailyRecordsSet>, List<DailyRecordsSet>, List<DailyRecordsSet>>
        {
            { DailyRecordsSetsListAprilNr2Id_2_3, DailyRecordsSetsListAprilNr2Id_4_5, DailyRecordsSetsListAprilNr2() },
            { new List<DailyRecordsSet>(), DailyRecordsSetsListAprilNr2(), DailyRecordsSetsListAprilNr2() },
            { DailyRecordsSetsListAprilNr2(), new List<DailyRecordsSet>(), DailyRecordsSetsListAprilNr2() }
        };

    [Theory]
    [MemberData(nameof(DataMakeUnionNotOverlaping))]
    public void MakeUnion_ShouldReturnUnionOfSets_WhenBothSetsAreNotOverlaping(List<DailyRecordsSet> sets1,
                                                                         List<DailyRecordsSet> sets2,
                                                                         List<DailyRecordsSet> expectedUnion)
    {
        // Arrange        

        // Act
        var result = _sut.MakeUnion(sets1, sets2);

        // Assert
        result.Should().BeEquivalentTo(expectedUnion);
    }

    [Fact]
    public void MakeUnion_ShouldReturnUnionOfSets_WhenSetsAreOverlaping()
    {
        // Arrange
        List<DailyRecordsSet> set1 = DailyRecordsSetsListAprilNr2Id_2_4;
        List<DailyRecordsSet> set2 = DailyRecordsSetsListAprilNr2Id_3_5;
        List<DailyRecordsSet> expectedUnion = DailyRecordsSetsListAprilNr2();

        DailyRecordsSet dailySet2_id3 = set2.Single(d => d.Id == 3);
        DailyRecordsSet resultOf_Id3 = expectedUnion.Single(x => x.Id == 3);

        DailyRecordsSet dailySet2_id4 = set2.Single(d => d.Id == 4);
        DailyRecordsSet resultOf_Id4 = expectedUnion.Single(x => x.Id == 4);

        foreach (var d in dailySet2_id3.Records)
        {
            resultOf_Id3.Records.Add(d);
        }
        foreach (var d in dailySet2_id4.Records)
        {
            resultOf_Id4.Records.Add(d);
        }
        foreach (var d in dailySet2_id3.MergedTags)
        {
            resultOf_Id3.MergedTags.Add(d);
        }
        foreach (var d in dailySet2_id4.MergedTags)
        {
            resultOf_Id4.MergedTags.Add(d);
        }
        resultOf_Id3.MergedContents += dailySet2_id3.MergedContents;
        resultOf_Id4.MergedContents += dailySet2_id4.MergedContents;

        // Act
        var result = _sut.MakeUnion(set1, set2);

        // Assert
        result.Should().BeEquivalentTo(expectedUnion);
    }

    [Fact]
    public void MakeUnion_ShouldReturnUnionOfSets_WhenSetsAreOverlaping_Test2()
    {
        // Arrange
        List<DailyRecordsSet> testSet1 = DailyRecordsSetsListAprilNr2Id_2_3;
        List<DailyRecordsSet> testSet2 = DailyRecordsSetsListAprilNr2Id_3_5;
        List<DailyRecordsSet> expectedUnion = DailyRecordsSetsListAprilNr2();

        DailyRecordsSet testItem2_id3 = testSet2.Single(d => d.Id == 3);
        testItem2_id3.EmailHasBeenSent = true;
        testItem2_id3.MergedTags = null;
        testItem2_id3.HasAnyTags = false;
        testItem2_id3.Records = new List<Domain.Entity.Record>();
        testItem2_id3.HasAnyRecords = false;
        testItem2_id3.MergedContents = "";
        testItem2_id3.RefersToDate = new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddMinutes(20);

        DailyRecordsSet expected_Id3 = expectedUnion.Single(x => x.Id == 3);

        foreach (var d in testItem2_id3.Records)
        {
            expected_Id3.Records.Add(d);
        }

        expected_Id3.MergedContents += testItem2_id3.MergedContents;
        expected_Id3.EmailHasBeenSent = true;

        // Act
        var result = _sut.MakeUnion(testSet1, testSet2);

        // Assert
        result.Should().BeEquivalentTo(expectedUnion);
    }

    [Fact]
    public void Equals_ShouldMergeSets_WhenDatesAreEqual()
    {
        // Arrange
        DailyRecordsSet set1 = DailyRecordsSetsMarch2023Nr1Id_3;
        DailyRecordsSet set2 = DailyRecordsSetsMarch2023Nr1Id_5;
        set1.RefersToDate = new DateTimeOffset(2023, 3, 7, 0, 0, 0, TimeSpan.Zero).AddHours(4);
        set2.RefersToDate = new DateTimeOffset(2023, 3, 7, 0, 0, 0, TimeSpan.Zero).AddHours(6);

        var expectedRecords = new List<Domain.Entity.Record>();

        foreach (var d in set1.Records)
        {
            expectedRecords.Add(d);
        }
        foreach (var d in set2.Records)
        {
            expectedRecords.Add(d);
        }
        var expectedContent = set1.MergedContents + set2.MergedContents;

        // Act
        var result = _sut.Equals(set1, set2);

        // Assert
        result.Should().BeTrue();
        set1.Id.Should().Be(set1.Id);
        set1.CreatedDateTime.Should().Be(set1.CreatedDateTime);
        set1.Records.Should().BeEquivalentTo(expectedRecords);
        set1.MergedContents.Should().Be(expectedContent);
        set1.EmailHasBeenSent.Should().Be(false);
        set1.HasAnyRecords.Should().Be(true);
        set1.HasAnyTags.Should().Be(true);
        set1.MergedTags.Should().BeEquivalentTo(set2.MergedTags);
    }

    [Fact]
    public void Equals_ShouldMergeSets_WhenDatesAreEqual_Test2()
    {
        // Arrange
        DailyRecordsSet set1 = DailyRecordsSetsMarch2023Nr1Id_2;
        DailyRecordsSet set2 = DailyRecordsSetsMarch2023Nr1Id_3;
        set1.RefersToDate = new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(4);
        set2.RefersToDate = new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(6);

        var expectedSet = DailyRecordsSetsMarch2023Nr1Id_2;

        foreach (var d in set2.Records)
        {
            expectedSet.Records.Add(d);
        }
        expectedSet.MergedContents += set2.MergedContents;

        // Act
        var result = _sut.Equals(set1, set2);

        // Assert
        result.Should().BeTrue();
        set1.Records.Should().BeEquivalentTo(expectedSet.Records);
        set1.MergedContents.Should().Be(expectedSet.MergedContents);
        set1.EmailHasBeenSent.Should().Be(expectedSet.EmailHasBeenSent);
        set1.HasAnyRecords.Should().Be(expectedSet.HasAnyRecords);
        set1.HasAnyTags.Should().Be(expectedSet.HasAnyTags);
        set1.MergedTags.Should().BeEquivalentTo(new List<Tag>());
    }

    public static TheoryData<List<DailyRecordsSet>, List<DailyRecordsSet>> DataEqualsDatesAreDifferent =>
       new TheoryData<List<DailyRecordsSet>, List<DailyRecordsSet>>
       {
           {DailyRecordsSetsMarch2023Nr1(), DailyRecordsSetsListAprilNr2() },
           {DailyRecordsSetsListNr1(), DailyRecordsSetsListAprilNr2() },
           {DailyRecordsSetsMarch2023Nr1(), DailyRecordsSetsListNr1() }
       };

    [Theory]
    [MemberData(nameof(DataEqualsDatesAreDifferent))]
    public void Equals_ShouldNotMergeSets_WhenDatesAreDifferent(List<DailyRecordsSet> sets1,
                                                                List<DailyRecordsSet> sets2)
    {
        foreach (var set1 in sets1)
        {
            foreach (var set2 in sets2)
            {
                // Arrange

                // Act
                var result = _sut.Equals(set1, set2);

                // Assert
                result.Should().BeFalse();
                set1.Id.Should().Be(set1.Id);
                set1.CreatedDateTime.Should().Be(set1.CreatedDateTime);
                set1.Records.Should().BeEquivalentTo(set1.Records);
                set1.MergedContents.Should().BeEquivalentTo(set1.MergedContents);
                set1.EmailHasBeenSent.Should().Be(set1.EmailHasBeenSent);
                set1.HasAnyRecords.Should().Be(set1.HasAnyRecords);
                set1.HasAnyTags.Should().Be(set1.HasAnyTags);
                set1.MergedTags.Should().BeEquivalentTo(set1.MergedTags);
            }
        }
    }

    [Fact]
    public void Equals_ShouldNotMergeSets_WhenDatesAreDifferent_Test2()
    {
        // Arrange
        var set1 = new DailyRecordsSet { RefersToDate = new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero) };
        var set2 = new DailyRecordsSet { RefersToDate = new DateTimeOffset(2023, 1, 2, 0, 0, 0, TimeSpan.Zero) };

        // Act
        var result = _sut.Equals(set1, set2);

        // Assert       

        result.Should().BeFalse();
        set1.Id.Should().Be(0);
        set1.CreatedDateTime.Should().Be(DateTimeOffset.MinValue);
        set1.Records.Should().BeNull();
        set1.MergedTags.Should().BeNull();
        set1.MergedContents.Should().BeEmpty();
        set1.EmailHasBeenSent.Should().BeFalse();
        set1.HasAnyRecords.Should().BeFalse();
        set1.HasAnyTags.Should().BeFalse();
    }

}
