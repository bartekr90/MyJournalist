using MyJournalist.Domain.Entity;

namespace MyJournalist.Worker.Tests.TestData;

internal static class SampleDataSet
{
    public static string[] contentArrayWithTags =
     {//7
        "Przykładowy tekst 2 $3 #drugi",
        "Inny tekst 1 $3 #pierwszy",
        "Losowy opis 1 $3 #opis",
        "Tekst numer 2 $3 #drugi",
        "Przykładowy opis 1 #opis $12",
        "Inny tekst 2 #drugi",
        "Opisowy tekst $1 #pierwszy #sendnow"
    };

    internal static DateTimeOffset[] DateArray = new DateTimeOffset[]
    {//15
        new DateTimeOffset(2023, 07, 01, 09, 50, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 01, 10, 05, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 01, 12, 00, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 01, 13, 23, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 02, 09, 50, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 02, 10, 05, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 03, 12, 00, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 03, 12, 23, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 04, 12, 23, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 04, 15, 23, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 04, 16, 23, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 05, 14, 00, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 05, 14, 23, 00, TimeSpan.Zero),

        new DateTimeOffset(2023, 07, 05, 09, 50, 00, TimeSpan.Zero),
        new DateTimeOffset(2023, 07, 05, 10, 05, 00, TimeSpan.Zero),
    };

    internal static Tag[] GetTagsArray => new Tag[]
    {
         new Tag
         {
            Name = "drugi",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[0],
            NameOfInitialRecord = "Rec_01/07/2023_09-50",
            TimeTokens = 3,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[1],
            ModifiedDateTime = DateArray[1],
            ModifiedById = null
         },
         new Tag
         {
            Name = "pierwszy",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[2],
            NameOfInitialRecord = "Rec_01/07/2023_12-00",
            TimeTokens = 3,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[3],
            ModifiedDateTime = DateArray[3],
            ModifiedById = null
         },
         new Tag
         {
            Name = "opis",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[4],
            NameOfInitialRecord = "Rec_02/07/2023_09-50",
            TimeTokens = 3,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[5],
            ModifiedDateTime = DateArray[5],
            ModifiedById = null
         },
         new Tag
         {
            Name = "drugi",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[6],
            NameOfInitialRecord = "Rec_03/07/2023_12-00",
            TimeTokens = 3,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[7],
            ModifiedDateTime = DateArray[7],
            ModifiedById = null
         },
         new Tag
         {
            Name = "opis",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[9],
            NameOfInitialRecord = "Rec_04/07/2023_15-23",
            TimeTokens = 12,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[10],
            ModifiedDateTime = DateArray[10],
            ModifiedById = null
         },
         new Tag
         {
            Name = "drugi",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[11],
            NameOfInitialRecord = "Rec_05/07/2023_14-00", 
            TimeTokens = 0,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[12],
            ModifiedDateTime = DateArray[12],
            ModifiedById = null
         },
         new Tag
         {
            Name = "pierwszy",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[13],
            NameOfInitialRecord = "Rec_05/07/2023_09-50",
            TimeTokens = 1,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[14],
            ModifiedDateTime = DateArray[14],
            ModifiedById = null
         },
         new Tag
         {
            Name = "sendnow",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[13],
            NameOfInitialRecord = "Rec_05/07/2023_09-50",
            TimeTokens = 1,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[14],
            ModifiedDateTime = DateArray[14],
            ModifiedById = null
         }
    };

    internal static Tag[] GetTagsArray2 => new Tag[]
   {
         new Tag
         {
            Name = "drugi",
            IdOfInitialRecord = 0,
            ContentDate = DateArray[0],
            NameOfInitialRecord = "Rec_01/07/2023_09-50",
            TimeTokens = 6,
            Records = null,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[1],
            ModifiedDateTime = DateArray[1],
            ModifiedById = null
         }
   };

    internal static Domain.Entity.Record[] GetRecorsArray => new Domain.Entity.Record[]
    {
        new Domain.Entity.Record
        {
            Name = "Rec_01/07/2023_09-50",
            Content = contentArrayWithTags[0],
            ContentDate = DateArray[0],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[0]
            },
            TimeTokens = 3,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[1],
            ModifiedDateTime = DateArray[1],
            ModifiedById = null
        },
        new Domain.Entity.Record
        {
            Name = "Rec_01/07/2023_12-00",
            Content = contentArrayWithTags[1],
            ContentDate = DateArray[2],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[1]
            },
            TimeTokens = 3,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[3],
            ModifiedDateTime = DateArray[3],
            ModifiedById = null
        },
        new Domain.Entity.Record
        {
            Name = "Rec_02/07/2023_09-50",
            Content = contentArrayWithTags[2],
            ContentDate = DateArray[4],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[2]
            },
            TimeTokens = 3,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[5],
            ModifiedDateTime = DateArray[5],
            ModifiedById = null
        },
        new Domain.Entity.Record
        {
            Name = "Rec_03/07/2023_12-00",
            Content = contentArrayWithTags[3],
            ContentDate = DateArray[6],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[3]
            },
            TimeTokens = 3,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[7],
            ModifiedDateTime = DateArray[7],
            ModifiedById = null
        },
        new Domain.Entity.Record
        {
            Name = "Rec_04/07/2023_15-23",
            Content = contentArrayWithTags[4],
            ContentDate = DateArray[9],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[4]
            },
            TimeTokens = 12,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[10],
            ModifiedDateTime = DateArray[10],
            ModifiedById = null
        },
        new Domain.Entity.Record
        {
            Name = "Rec_05/07/2023_14-00",
            Content = contentArrayWithTags[5],
            ContentDate = DateArray[11],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[5]
            },
            TimeTokens = 0,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[12],
            ModifiedDateTime = DateArray[12],
            ModifiedById = null
        },
        new Domain.Entity.Record
        {
            Name = "Rec_05/07/2023_09-50",
            Content = contentArrayWithTags[6],
            ContentDate = DateArray[13],
            HasContent = true,
            HasAnyTags = true,
            Tags = new List<Tag>
            {
                GetTagsArray[6], GetTagsArray[7]
            },
            TimeTokens = 1,
            Id = 0,
            CreatedById = 0,
            CreatedDateTime = DateArray[14],
            ModifiedDateTime = DateArray[14],
            ModifiedById = null
        }
    };

    internal static List<Domain.Entity.Record> GetRecorsList => new List<Domain.Entity.Record>
    {
        GetRecorsArray[0], GetRecorsArray[1], GetRecorsArray[2], GetRecorsArray[3]
    };

    internal static DailyRecordsSet[] GetDailyRecordsSetsArray = new DailyRecordsSet[]
    {
        new DailyRecordsSet
        {
            Records = new List<Domain.Entity.Record>
            {
                GetRecorsArray[0], GetRecorsArray[1]
            },
            RefersToDate = DateArray[0].Date,
            EmailHasBeenSent = true,
            HasAnyRecords = true,
            MergedContents = $@"{contentArrayWithTags[0]}

***

{contentArrayWithTags[1]}

***

",
            MergedTags = new List<Tag>
            {
                GetTagsArray[0], GetTagsArray[1]
            },
            HasAnyTags = true,
            CreatedDateTime = DateArray[8],
            ModifiedDateTime = DateArray[8],
        },
        new DailyRecordsSet
        {
            Records = new List<Domain.Entity.Record>
            {
               GetRecorsArray[2]
            },
            RefersToDate = DateArray[4].Date,
            EmailHasBeenSent = true,
            HasAnyRecords = true,
            MergedContents = $@"{contentArrayWithTags[2]}

***

",
            MergedTags = new List<Tag>
            {
               GetTagsArray[2]
            },
            HasAnyTags = true,
            CreatedDateTime = DateArray[8],
            ModifiedDateTime = DateArray[8],
        },
        new DailyRecordsSet
        {
            Records = new List<Domain.Entity.Record>
            {
                GetRecorsArray[3]
            },
            RefersToDate = DateArray[6].Date,
            EmailHasBeenSent = true,
            HasAnyRecords = true,
            MergedContents = $@"{contentArrayWithTags[3]}

***

",
            MergedTags = new List<Tag>
            {
                GetTagsArray[3]
            },
            HasAnyTags = true,
            CreatedDateTime = DateArray[8],
            ModifiedDateTime = DateArray[8],
        }
    };

    internal static DailyRecordsSet[] GetDailyRecordsSetsArray2 = new DailyRecordsSet[]
    {
        new DailyRecordsSet
        {
            Records = new List<Domain.Entity.Record>
            {
                GetRecorsArray[0], GetRecorsArray[1]
            },
            RefersToDate = DateArray[0].Date,
            EmailHasBeenSent = true,
            HasAnyRecords = true,
            MergedContents = $@"{contentArrayWithTags[0]}

***

{contentArrayWithTags[1]}

***

",
            MergedTags = new List<Tag>
            {
                GetTagsArray[0], GetTagsArray[1]
            },
            HasAnyTags = true,
            CreatedDateTime = DateArray[1],
            ModifiedDateTime = DateArray[1],
        },
        new DailyRecordsSet
        {
            Records = new List<Domain.Entity.Record>
            {
               GetRecorsArray[2]
            },
            RefersToDate = DateArray[4].Date,
            EmailHasBeenSent = true,
            HasAnyRecords = true,
            MergedContents = $@"{contentArrayWithTags[2]}

***

",
            MergedTags = new List<Tag>
            {
               GetTagsArray[2]
            },
            HasAnyTags = true,
            CreatedDateTime = DateArray[5],
            ModifiedDateTime = DateArray[5],
        },
        new DailyRecordsSet
        {
            Records = new List<Domain.Entity.Record>
            {
                GetRecorsArray[3]
            },
            RefersToDate = DateArray[6].Date,
            EmailHasBeenSent = true,
            HasAnyRecords = true,
            MergedContents = $@"{contentArrayWithTags[3]}

***

",
            MergedTags = new List<Tag>
            {
                GetTagsArray[3]
            },
            HasAnyTags = true,
            CreatedDateTime = DateArray[7],
            ModifiedDateTime = DateArray[7],
        }
    };
    
    internal static List<DailyRecordsSet> GetDailyRecordsSets => new List<DailyRecordsSet>
    {
        GetDailyRecordsSetsArray[0], GetDailyRecordsSetsArray[1], GetDailyRecordsSetsArray[2]
    };

}