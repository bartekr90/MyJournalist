using MyJournalist.Domain.Entity;
using System.Text;

namespace MyJournalist.App.Tests;

internal class SampleLists
{
    //*******************************************************************************************************
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff1Min => tupleArray[0];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff0SecMin => tupleArray[1];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff0SecMax => tupleArray[2];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff2000Yers => tupleArray[3];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff9998Yers => tupleArray[4];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff34Hour => tupleArray[5];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff59Sec => tupleArray[6];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff40Days => tupleArray[7];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff2Hours => tupleArray[8];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff1Day => tupleArray[9];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff8Hours => tupleArray[10];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff89Days => tupleArray[11];
    public static Tuple<DateTimeOffset, DateTimeOffset> GetDateSetDiff3Years => tupleArray[12];

    //*******************************************************************************************************
    public static List<Domain.Entity.Record> RecordsListNr1()
    {
        return new List<Domain.Entity.Record>
        {
           new Domain.Entity.Record
            {
                Id = 1,
                CreatedById = 1,
                CreatedDateTime = DateTimeOffset.Now,
                ModifiedById = 1,
                ModifiedDateTime = DateTimeOffset.Now,
                Name = "Record 1",
                Content = "Content for Record 1",
                ContentDate = DateTimeOffset.Now,
                HasContent = true,
                HasAnyTags = true,
                Tags = new List<Tag> { new Tag { Name = "Tag 1" }, new Tag { Name = "Tag 2" } },
                TimeTokens = 10
            },
            new Domain.Entity.Record
            {
                    Id = 2,
                CreatedById = 2,
                CreatedDateTime = DateTimeOffset.Now.AddDays(-2),
                ModifiedById = 2,
                ModifiedDateTime = DateTimeOffset.Now.AddDays(-2),
                Name = "Record 2",
                Content = "Content for Record 2",
                ContentDate = DateTimeOffset.Now.AddDays(-3),
                HasContent = true,
                HasAnyTags = false,
                Tags = null,
                TimeTokens = null
            },
            new Domain.Entity.Record
            {
                Id = 3,
                CreatedById = 3,
                CreatedDateTime = DateTimeOffset.Now.AddDays(-3),
                ModifiedById = 3,
                ModifiedDateTime = DateTimeOffset.Now.AddDays(-3),
                Name = "Record 3",
                Content = string.Empty,
                ContentDate = DateTimeOffset.Now.AddDays(-3),
                HasContent = false,
                HasAnyTags = false,
                Tags = new List<Tag>(),
                TimeTokens = 5
            }
        };
    }
    public static string GetMergedContentNr1 =>
        @"Content for Record 1

***

Content for Record 2

***

";
    public static List<Tag> GetTagListNr1()
    {
        return new List<Tag>
        {
            new Tag
            {
                CreatedById = 1,
                CreatedDateTime = DateTimeOffset.Now,
                ModifiedById = 1,
                ModifiedDateTime = DateTimeOffset.Now,
                Name = "Tag 1",
                IdOfInitialRecord = 1,
                ContentDate = DateTimeOffset.Now.AddDays(-1),
                NameOfInitialRecord = "Record 1",
                TimeTokens = 10,
            },
            new Tag
            {
                CreatedById = 2,
                CreatedDateTime = DateTimeOffset.Now.AddDays(-2),
                ModifiedById = 2,
                ModifiedDateTime = DateTimeOffset.Now.AddDays(-2),
                Name = "Tag 2",
                IdOfInitialRecord = 2,
                ContentDate = DateTimeOffset.Now.AddDays(-3),
                NameOfInitialRecord = "Record 2",
                TimeTokens = 5,
                Records = null
            },
            new Tag
            {
                CreatedById = 3,
                CreatedDateTime = DateTimeOffset.Now.AddDays(-3),
                ModifiedById = 3,
                ModifiedDateTime = DateTimeOffset.Now.AddDays(-3),
                Name = "Tag 3",
                IdOfInitialRecord = 3,
                ContentDate = DateTimeOffset.Now.AddDays(-3),
                NameOfInitialRecord = "Record 3",
                TimeTokens = 8,
            }
        };
    }

    //*******************************************************************************************************

    public static List<Domain.Entity.Record> RecordsListNr2April()
    {
        return new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record
            {
                Id = 1,
                CreatedById = 1,
                CreatedDateTime = new DateTimeOffset(2023, 4, 2, 18, 31, 48, TimeSpan.FromHours(2)),
                ModifiedById = 1,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 2, 18, 31, 48, TimeSpan.FromHours(2)),
                Name = "Rec_27/04/2023_18-31",
                Content = "Test za 27 o4",
                ContentDate = new DateTimeOffset(2023, 4, 1, 1, 31, 1, TimeSpan.FromHours(2)),
                HasContent = true,
                TimeTokens = 1,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                Id = 2,
                CreatedById = 2,
                CreatedDateTime = new DateTimeOffset(2023, 4, 4, 18, 32, 3, TimeSpan.FromHours(2)),
                ModifiedById = 2,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 4, 18, 32, 3, TimeSpan.FromHours(2)),
                Name = "Rec_27/04/2023_18-31",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 4, 3, 3, 31, 48, TimeSpan.FromHours(2)),
                HasContent = false,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                 Id = 3,
                CreatedById = 3,
                CreatedDateTime = new DateTimeOffset(2023, 4, 5, 18, 33, 10, TimeSpan.FromHours(2)),
                ModifiedById = 3,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 5, 18, 33, 10, TimeSpan.FromHours(2)),
                Name = "Rec_27/04/2023_18-31",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 4, 5, 5, 31, 48, TimeSpan.FromHours(2)),
                HasContent = false,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                 Id = 4,
                CreatedById = 4,
                CreatedDateTime = new DateTimeOffset(2023, 4, 6, 12, 15, 0, TimeSpan.FromHours(0)),
                ModifiedById = 4,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 6, 12, 15, 0, TimeSpan.FromHours(0)),
                Name = "Rec_27/04/2023_18-31",
                Content = "Lorem ipsum dolor sit amet.",
                ContentDate = new DateTimeOffset(2023, 4, 6, 10, 0, 0, TimeSpan.FromHours(0)),
                HasContent = true,
                TimeTokens = 3,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                 Id = 5,
                CreatedById = 5,
                CreatedDateTime = new DateTimeOffset(2023, 4, 7, 8, 0, 0, TimeSpan.FromHours(0)),
                ModifiedById = 5,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 7, 8, 0, 0, TimeSpan.FromHours(0)),
                Name = "Rec_27/04/2023_18-31",
                Content = "Sample content.",
                ContentDate = new DateTimeOffset(2023, 4, 7, 7, 30, 0, TimeSpan.FromHours(0)),
                HasContent = true,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                Id = 6,
                CreatedById = 6,
                CreatedDateTime = new DateTimeOffset(2023, 4, 21, 14, 45, 0, TimeSpan.FromHours(0)),
                ModifiedById = 6,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 21, 14, 45, 0, TimeSpan.FromHours(0)),
                Name = "Rec_27/04/2023_18-31",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 4, 16, 14, 0, 0, TimeSpan.FromHours(0)),
                HasContent = false,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                 Id = 7,
                CreatedById = 7,
                CreatedDateTime = new DateTimeOffset(2023, 4, 23, 20, 0, 0, TimeSpan.FromHours(0)),
                ModifiedById = 7,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 23, 20, 0, 0, TimeSpan.FromHours(0)),
                Name = "Rec_27/04/2023_18-31",
                Content = "Content for the evening.",
                ContentDate = new DateTimeOffset(2023, 4, 27, 19, 30, 0, TimeSpan.FromHours(0)),
                HasContent = true,
                HasAnyTags = false
            }

        };
    }
    public static string GetMergedContentFromApril =>
        @"Test za 27 o4

***

Lorem ipsum dolor sit amet.

***

Sample content.

***

Content for the evening.

***

";

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordsListNr3Empty()
    {
        return new List<Domain.Entity.Record>
        {
            new Domain.Entity.Record
            {
                Id = 1,
                CreatedById = 3,
                CreatedDateTime = new DateTimeOffset(2023, 4, 26, 18, 33, 10, TimeSpan.FromHours(2)),
                ModifiedById = 3,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 26, 18, 33, 10, TimeSpan.FromHours(2)),
                Name = "Rec_26/04/2023_18-30",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 4, 25, 5, 31, 48, TimeSpan.FromHours(2)),
                HasContent = false,
                HasAnyTags = false
            },
            new Domain.Entity.Record
            {
                Id = 2,
                CreatedById = 3,
                CreatedDateTime = new DateTimeOffset(2023, 4, 27, 18, 33, 10, TimeSpan.FromHours(2)),
                ModifiedById = 3,
                ModifiedDateTime = new DateTimeOffset(2023, 4, 27, 18, 33, 10, TimeSpan.FromHours(2)),
                Name = "Rec_27/04/2023_18-31",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 4, 27, 5, 31, 48, TimeSpan.FromHours(2)),
                HasContent = false,
                HasAnyTags = false
            }
        };
    }

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordListMarch2023nr1()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 1; i <= 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 1,
                CreatedById = i,
                CreatedDateTime = tupleArray[12 + i].Item2,
                ModifiedById = i,
                ModifiedDateTime = tupleArray[12 + i].Item2,
                Name = "Rec_From_01march_list1",
                ContentDate = tupleArray[12 + i].Item1,
                HasContent = true,
                HasAnyTags = false,
                Content = contentArray[i - 1]
            };
            list.Add(record);
        }
        return list;
    }
    public static string ContentMergedMarch2023nr1 =>
        @$"{contentArray[0]}

***

{contentArray[1]}

***

{contentArray[2]}

***

{contentArray[3]}

***

";

    //*******************************************************************************************************

    public static List<Domain.Entity.Record> RecordListMarch2023nr2()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 1; i <= 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 2,
                CreatedById = i,
                CreatedDateTime = tupleArray[16 + i].Item2,
                ModifiedById = i,
                ModifiedDateTime = tupleArray[16 + i].Item2,
                Name = "Rec_From_02march_list2",
                ContentDate = tupleArray[16 + i].Item1,
                HasContent = true,
                HasAnyTags = false,
                Content = contentArray[3 + i]
            };
            list.Add(record);
        }
        list.Add(
            new Domain.Entity.Record
            {
                Id = 521,
                CreatedById = 3,
                CreatedDateTime = new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(9),
                ModifiedById = 3,
                ModifiedDateTime = new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(9),
                Name = "Rec_From_02march_list2_empty",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(8),
                HasContent = false,
                HasAnyTags = false
            });

        return list;
    }
    public static string ContentMergedMarch2023nr2 =>
        @$"{contentArray[4]}

***

{contentArray[5]}

***

{contentArray[6]}

***

{contentArray[7]}

***

";
    public static List<Domain.Entity.Record> RecordSubListMarch2023nr2Id_3_4 =>
        RecordListMarch2023nr2().Where(r => r.Id <= 4).ToList();
    public static List<Domain.Entity.Record> RecordSubListMarch2023nr2Id_5_6 =>
        RecordListMarch2023nr2().Where(r => r.Id > 4 && r.Id <= 6).ToList();
    public static List<Domain.Entity.Record> RecordSubListMarch2023nr2Id_4_6 =>
        RecordListMarch2023nr2().Where(r => r.Id > 3 && r.Id <= 6).ToList();
    public static List<Domain.Entity.Record> RecordSubListMarch2023nr2Id_521 =>
        RecordListMarch2023nr2().Where(r => r.Id == 521).ToList();

    //******************************************************************************************************

    public static List<Domain.Entity.Record> RecordListMarch2023nr3()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 1; i <= 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 3,
                CreatedById = i,
                CreatedDateTime = tupleArray[20 + i].Item2,
                ModifiedById = i,
                ModifiedDateTime = tupleArray[20 + i].Item2,
                Name = "Rec_From_03march_list3",
                ContentDate = tupleArray[20 + i].Item1,
                HasContent = true,
                HasAnyTags = false,
                Content = contentArray[7 + i]
            };
            list.Add(record);
        }
        list.Add(
            new Domain.Entity.Record
            {
                Id = 5,
                CreatedById = 3,
                CreatedDateTime = new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(9),
                ModifiedById = 3,
                ModifiedDateTime = new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(9),
                Name = "Rec_From_03march_list3_empty",
                Content = "[no content]",
                ContentDate = new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(8),
                HasContent = false,
                HasAnyTags = false
            });

        return list;
    }
    public static string ContentMergedMarch2023nr3 =>
        @$"{contentArray[8]}

***

{contentArray[9]}

***

{contentArray[10]}

***

{contentArray[11]}

***

";

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordListMarch2023nr4()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 1; i <= 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 4,
                CreatedById = i,
                CreatedDateTime = tupleArray[24 + i].Item2,
                ModifiedById = i,
                ModifiedDateTime = tupleArray[24 + i].Item2,
                Name = "Rec_From_04march_list4",
                ContentDate = tupleArray[24 + i].Item1,
                HasContent = true,
                HasAnyTags = false,
                Content = contentArray[11 + i]
            };
            list.Add(record);
        }

        return list;
    }
    public static string ContentMergedMarch2023nr4 =>
        @$"{contentArray[12]}

***

{contentArray[13]}

***

{contentArray[14]}

***

{contentArray[15]}

***

";

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordListMarch2023TagsTokensNr5()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 0; i < 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 1 + 5,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[29 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[29 + i].Item2,
                Name = "Rec_From_05march_list5",
                ContentDate = tupleArray[29 + i].Item1,
                HasContent = true,
                HasAnyTags = true,
                Tags = new List<Tag>(),
                TimeTokens = 3,
                Content = contentArrayWithTags[i]
            };
            var tag = new Tag
            {
                Id = i + 1 + 5,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[29 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[29 + i].Item2,
                ContentDate = tupleArray[29 + i].Item1,
                TimeTokens = 3,
                IdOfInitialRecord = i + 1 + 5,
                NameOfInitialRecord = "Rec_From_05march_list5",
                Name = tagsArray[i],
            };
            record.Tags.Add(tag);

            list.Add(record);
        }

        return list;
    }
    public static string ContentMergedMarch2023TagsTokensNr5 =>
        @$"{contentArrayWithTags[0]}

***

{contentArrayWithTags[1]}

***

{contentArrayWithTags[2]}

***

{contentArrayWithTags[3]}

***

";

    public static List<Tag> TagsMarch2023TokensNr5()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 4; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 5,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[29 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[29 + i].Item2,
                ContentDate = tupleArray[29 + i].Item1,
                TimeTokens = 3,
                IdOfInitialRecord = i + 1 + 5,
                NameOfInitialRecord = "Rec_From_05march_list5",
                Name = tagsArray[i],
            };

            list.Add(tag);
        }

        return list;
    }
    public static List<Tag> TagsMergedMarch2023TokensNr5()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 3; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 5,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[29 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[29 + i].Item2,
                ContentDate = tupleArray[29 + i].Item1,
                TimeTokens = i == 0 ? 6u : 3u,
                IdOfInitialRecord = i + 1 + 5,
                NameOfInitialRecord = "Rec_From_05march_list5",
                Name = tagsArray[i],
            };

            list.Add(tag);
        }

        return list;
    }

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordList01April2023TagsNr6()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 0; i < 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 1 + 9,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[33 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[33 + i].Item2,
                Name = "Rec_From_01April_list6",
                ContentDate = tupleArray[33 + i].Item1,
                HasContent = true,
                HasAnyTags = true,
                Tags = new List<Tag>(),
                Content = contentArrayWithTags[4 + i]
            };
            var tag = new Tag
            {
                Id = i + 1 + 9,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[33 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[33 + i].Item2,
                ContentDate = tupleArray[33 + i].Item1,
                IdOfInitialRecord = i + 1 + 6,
                NameOfInitialRecord = "Rec_From_01April_list6",
                Name = tagsArray[4 + i],
            };
            record.Tags.Add(tag);

            list.Add(record);
        }

        return list;
    }
    public static string ContentMerged01April2023TagsNr6 =>
        @$"{contentArrayWithTags[4]}

***

{contentArrayWithTags[5]}

***

{contentArrayWithTags[6]}

***

{contentArrayWithTags[7]}

***

";
    public static List<Tag> TagsMerged01April2023Nr6()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 4; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 9,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[33 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[33 + i].Item2,
                ContentDate = tupleArray[33 + i].Item1,
                IdOfInitialRecord = i + 1 + 6,
                NameOfInitialRecord = "Rec_From_01April_list6",
                Name = tagsArray[4 + i],
            };
            list.Add(tag);
        }
        return list;
    }

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordList02April2023TagsNr7()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 0; i < 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 1 + 7,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[37 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[37 + i].Item2,
                Name = "Rec_From_02April_list7",
                ContentDate = tupleArray[37 + i].Item1,
                HasContent = true,
                HasAnyTags = true,
                Tags = new List<Tag>(),
                Content = contentArrayWithTags[8 + i]
            };
            var tag = new Tag
            {
                Id = i + 1 + 7,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[37 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[37 + i].Item2,
                ContentDate = tupleArray[37 + i].Item1,
                IdOfInitialRecord = i + 1 + 7,
                NameOfInitialRecord = "Rec_From_02April_list7",
                Name = tagsArray[8 + i],
            };
            record.Tags.Add(tag);
            list.Add(record);
        }
        return list;
    }
    public static string ContentMerged02April2023TagsNr7 =>
        @$"{contentArrayWithTags[8]}

***

{contentArrayWithTags[9]}

***

{contentArrayWithTags[10]}

***

{contentArrayWithTags[11]}

***

";

    public static List<Tag> Tags02April2023Nr7()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 4; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 13,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[37 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[37 + i].Item2,
                ContentDate = tupleArray[37 + i].Item1,
                IdOfInitialRecord = i + 1 + 7,
                NameOfInitialRecord = "Rec_From_02April_list7",
                Name = tagsArray[8 + i],
            };
            list.Add(tag);
        }
        return list;
    }
    public static List<Tag> TagsMerged02April2023Nr7 =>
        new List<Tag>
        {
            new Tag
            {
                ContentDate = tupleArray[37].Item1,
                CreatedById = 1,
                CreatedDateTime = tupleArray[37].Item2,
                Id = 8,
                IdOfInitialRecord = 8,
                ModifiedById = 1,
                ModifiedDateTime = tupleArray[37].Item2,
                Name = "przykład",
                NameOfInitialRecord = "Rec_From_02April_list7",
                Records = null,
                TimeTokens = 0u
            },
             new Tag
            {
                ContentDate = tupleArray[38].Item1,
                CreatedById = 2,
                CreatedDateTime =tupleArray[38].Item2,
                Id = 9,
                IdOfInitialRecord = 9,
                ModifiedById = 2,
                ModifiedDateTime = tupleArray[38].Item2,
                Name = "tekst",
                NameOfInitialRecord = "Rec_From_02April_list7",
                Records = null,
                TimeTokens = 0u
            },
             new Tag
            {
                ContentDate = tupleArray[40].Item1,
                CreatedById = 4,
                CreatedDateTime = tupleArray[40].Item2,
                Id = 11,
                IdOfInitialRecord = 11,
                ModifiedById = 4,
                ModifiedDateTime = tupleArray[40].Item2,
                Name = "opis",
                NameOfInitialRecord = "Rec_From_02April_list7",
                Records = null,
                TimeTokens = 0u
            }
        };

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordList03April2023TagsNr8()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 0; i < 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 1 + 17,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[41 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[41 + i].Item2,
                Name = "Rec_From_03April_list8",
                ContentDate = tupleArray[41 + i].Item1,
                HasContent = true,
                HasAnyTags = true,
                Tags = new List<Tag>(),
                Content = contentArrayWithTags[12 + i],
                TimeTokens = 5
            };
            var tag = new Tag
            {
                Id = i + 1 + 17,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[41 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[41 + i].Item2,
                ContentDate = tupleArray[41 + i].Item1,
                IdOfInitialRecord = i + 1 + 8,
                NameOfInitialRecord = "Rec_From_03April_list8",
                Name = tagsArray[12 + i],
                TimeTokens = 5
            };
            record.Tags.Add(tag);
            list.Add(record);
        }
        return list;
    }
    public static string ContentMerged03April2023TagsNr8 =>
        @$"{contentArrayWithTags[12]}

***

{contentArrayWithTags[13]}

***

{contentArrayWithTags[14]}

***

{contentArrayWithTags[15]}

***

";
    public static List<Tag> TagsMerged03April2023Nr8()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 4; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 17,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[41 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[41 + i].Item2,
                ContentDate = tupleArray[41 + i].Item1,
                IdOfInitialRecord = i + 1 + 8,
                NameOfInitialRecord = "Rec_From_03April_list8",
                Name = tagsArray[12 + i],
                TimeTokens = 5
            };
            list.Add(tag);
        }
        return list;
    }

    //******************************************************************************************************
    public static List<Domain.Entity.Record> RecordList04April2023TagsNr9()
    {
        var list = new List<Domain.Entity.Record>();

        for (int i = 0; i < 4; i++)
        {
            var record = new Domain.Entity.Record
            {
                Id = i + 1 + 9,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[45 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[45 + i].Item2,
                Name = "Rec_From_04April_list9",
                ContentDate = tupleArray[45 + i].Item1,
                HasContent = true,
                HasAnyTags = true,
                Tags = new List<Tag>(),
                Content = contentArrayWithTags[16 + i]
            };
            var tag = new Tag
            {
                Id = i + 1 + 9,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[45 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[45 + i].Item2,
                ContentDate = tupleArray[45 + i].Item1,
                IdOfInitialRecord = i + 1 + 9,
                NameOfInitialRecord = "Rec_From_04April_list9",
                Name = tagsArray[16 + i],
            };
            record.Tags.Add(tag);
            list.Add(record);
        }
        return list;
    }
    public static string ContentMerged04April2023TagsNr9 =>
        @$"{contentArrayWithTags[16]}

***

{contentArrayWithTags[17]}

***

{contentArrayWithTags[18]}

***

{contentArrayWithTags[19]}

***

";
    public static List<Tag> Tags04April2023Nr9()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 4; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 21,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[45 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[45 + i].Item2,
                ContentDate = tupleArray[45 + i].Item1,
                IdOfInitialRecord = i + 1 + 9,
                NameOfInitialRecord = "Rec_From_04April_list9",
                Name = tagsArray[16 + i],
            };
            list.Add(tag);
        }
        return list;
    }
    public static List<Tag> TagsMerged04April2023Nr9 =>
        new List<Tag>
        {
            new Tag
            {
                ContentDate = tupleArray[45].Item1,
                CreatedById = 1,
                CreatedDateTime = tupleArray[45].Item2,
                Id = 10,
                IdOfInitialRecord = 10,
                ModifiedById = 1,
                ModifiedDateTime = tupleArray[45].Item2,
                Name = "opis",
                NameOfInitialRecord = "Rec_From_04April_list9",
                Records = null,
                TimeTokens = 0u
            },
            new Tag
            {
                ContentDate = tupleArray[47].Item1,
                CreatedById = 3,
                CreatedDateTime = tupleArray[47].Item2,
                Id = 12,
                IdOfInitialRecord = 12,
                ModifiedById = 3,
                ModifiedDateTime = tupleArray[47].Item2,
                Name = "pierwszy",
                NameOfInitialRecord = "Rec_From_04April_list9",
                Records = null,
                TimeTokens = 0u
            },
            new Tag
            {
                ContentDate = tupleArray[48].Item1,
                CreatedById = 4,
                CreatedDateTime = tupleArray[48].Item2,
                Id = 13,
                IdOfInitialRecord = 13,
                ModifiedById = 4,
                ModifiedDateTime = tupleArray[48].Item2,
                Name = "trzeci",
                NameOfInitialRecord = "Rec_From_04April_list9",
                Records = null,
                TimeTokens = 0u
            }
        };

    //******************************************************************************************************
    public static List<DailyRecordsSet> DailyRecordsSetsListNr1()
    {
        return new List<DailyRecordsSet>
        {
            new DailyRecordsSet()
            {
                 Id = 1,
                CreatedById = 123,
                CreatedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                ModifiedById = 123,
                ModifiedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                Records = RecordsListNr1(),
                RefersToDate = new DateTimeOffset(2023, 5, 11, 0, 0, 0, TimeSpan.Zero),
                MergedTags = GetTagListNr1(),
                MergedContents = GetMergedContentNr1,
                EmailHasBeenSent = false,
                HasAnyRecords = false,
                HasAnyTags = false
            },
            new DailyRecordsSet()
            {
                Id = 2,
                CreatedById = 456,
                CreatedDateTime = new DateTimeOffset(2023, 5, 12, 14, 30, 0, TimeSpan.Zero),
                ModifiedById = 456,
                ModifiedDateTime = new DateTimeOffset(2023, 5, 12, 14, 30, 0, TimeSpan.Zero),
                Records = RecordsListNr2April(),
                RefersToDate = new DateTimeOffset(2023, 5, 12, 0, 0, 0, TimeSpan.Zero),
                MergedTags = new List<Tag>(),
                MergedContents = GetMergedContentFromApril,
                EmailHasBeenSent = false,
                HasAnyRecords = false,
                HasAnyTags = false
            },
            new DailyRecordsSet()
            {
                Id = 3,
                CreatedById = 789,
                CreatedDateTime = new DateTimeOffset(2023, 5, 13, 19, 45, 0, TimeSpan.Zero),
                ModifiedById = 789,
                ModifiedDateTime = new DateTimeOffset(2023, 5, 13, 19, 45, 0, TimeSpan.Zero),
                Records = RecordsListNr3Empty(),
                RefersToDate = new DateTimeOffset(2023, 5, 13, 0, 0, 0, TimeSpan.Zero),
                MergedTags = null,
                MergedContents = "no content",
                EmailHasBeenSent = false,
                HasAnyRecords = true,
                HasAnyTags = false
            },
            new DailyRecordsSet()
            {
                 Id = 4,
                CreatedById = 987,
                CreatedDateTime = new DateTimeOffset(2023, 5, 14, 10, 15, 0, TimeSpan.Zero),
                ModifiedById = 987,
                ModifiedDateTime = new DateTimeOffset(2023, 5, 14, 10, 15, 0, TimeSpan.Zero),
                Records = null,
                RefersToDate = new DateTimeOffset(2023, 5, 14, 0, 0, 0, TimeSpan.Zero),
                MergedTags = null,
                MergedContents = "no content",
                EmailHasBeenSent = false,
                HasAnyRecords = false,
                HasAnyTags = false

            }

        };
    }

    //******************************************************************************************************
    public static List<DailyRecordsSet> DailyRecordsSetsMarch2023Nr1()
    {
        int n = 4;

        List<Domain.Entity.Record>[] listArray =
        {
            RecordListMarch2023nr1(),
            RecordListMarch2023nr2(),
            RecordListMarch2023nr3(),
            RecordListMarch2023nr4(),
            RecordListMarch2023TagsTokensNr5()
        };
        string[] contentArray =
        {
            ContentMergedMarch2023nr1,
            ContentMergedMarch2023nr2,
            ContentMergedMarch2023nr3,
            ContentMergedMarch2023nr4,
            ContentMergedMarch2023TagsTokensNr5
        };

        var list = new List<DailyRecordsSet>();
        for (int i = 0; i < n; i++)
        {
            var record = new DailyRecordsSet
            {
                Id = i + 1,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[16 + i * n].Item2,// tutaj ostatnia data utworzenia recodu
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[16 + i * n].Item2,
                Records = listArray[i],
                HasAnyRecords = true,
                MergedContents = contentArray[i],
                RefersToDate = tupleArray[13 + i * n].Item1, // refers to date to data ContentDate.Date recordu!
                MergedTags = null,
                HasAnyTags = false,
                EmailHasBeenSent = false
            };
            list.Add(record);
        }
        var recordWithTags = new DailyRecordsSet
        {
            Id = 5,
            CreatedById = 5,
            CreatedDateTime = tupleArray[32].Item2,// tutaj ostatnia data utworzenia recodu
            ModifiedById = 5,
            ModifiedDateTime = tupleArray[32].Item2,
            Records = listArray[4],
            HasAnyRecords = true,
            MergedContents = contentArray[4],
            RefersToDate = tupleArray[29].Item1, // refers to date to data ContentDate.Date recordu!
            MergedTags = TagsMarch2023TokensNr5(),
            HasAnyTags = true,
            EmailHasBeenSent = false
        };
        list.Add(recordWithTags);
        return list;
    }
    public static DailyRecordsSet DailyRecordsSetsMarch2023Nr1Id_2 =>
        DailyRecordsSetsMarch2023Nr1().Single(r => r.Id == 2);
    public static DailyRecordsSet DailyRecordsSetsMarch2023Nr1Id_3 =>
        DailyRecordsSetsMarch2023Nr1().Single(r => r.Id == 3);
    public static DailyRecordsSet DailyRecordsSetsMarch2023Nr1Id_5 =>
        DailyRecordsSetsMarch2023Nr1().Single(r => r.Id == 5);
    public static List<Domain.Entity.Record> CombinedListsFromNr1ToNr5 =>
        RecordListMarch2023nr1().Concat(RecordListMarch2023nr2()).Concat(RecordListMarch2023nr3()).Concat(RecordListMarch2023nr4()).Concat(RecordListMarch2023TagsTokensNr5()).ToList();
    public static List<Domain.Entity.Record> CombinedListsFromNr2ToNr5 =>
        RecordListMarch2023nr2().Concat(RecordListMarch2023nr3()).Concat(RecordListMarch2023nr4()).Concat(RecordListMarch2023TagsTokensNr5()).ToList();

    //******************************************************************************************************
    public static List<DailyRecordsSet> DailyRecordsSetsListAprilNr2()
    {
        int n = 4;

        List<Domain.Entity.Record>[] listArray =
        {
            RecordList01April2023TagsNr6(),
            RecordList02April2023TagsNr7(),
            RecordList03April2023TagsNr8(),
            RecordList04April2023TagsNr9(),
        };
        string[] contentArray =
        {
            ContentMerged01April2023TagsNr6,
            ContentMerged02April2023TagsNr7,
            ContentMerged03April2023TagsNr8,
            ContentMerged04April2023TagsNr9,
        };
        List<Tag>[] tagArray =
        {
            TagsMerged01April2023Nr6(),
            Tags02April2023Nr7(),
            TagsMerged03April2023Nr8(),
            Tags04April2023Nr9(),
        };

        var list = new List<DailyRecordsSet>();
        for (int i = 0; i < n; i++)
        {
            var record = new DailyRecordsSet
            {
                Id = i + 2,
                CreatedById = i + 2,
                CreatedDateTime = tupleArray[36 + i * n].Item2,// tutaj ostatnia data utworzenia recodu
                ModifiedById = i + 2,
                ModifiedDateTime = tupleArray[36 + i * n].Item2,
                Records = listArray[i],
                HasAnyRecords = true,
                MergedContents = contentArray[i],
                RefersToDate = tupleArray[33 + i * n].Item1, // refers to date to data ContentDate.Date recordu!
                MergedTags = tagArray[i],
                HasAnyTags = true,
                EmailHasBeenSent = false
            };
            list.Add(record);
        }
        return list;

    }

    public static List<DailyRecordsSet> DailyRecordsSetsListAprilNr2Id_2_3 =>
        DailyRecordsSetsListAprilNr2().Where(r => r.Id <= 3).ToList();
    public static List<DailyRecordsSet> DailyRecordsSetsListAprilNr2Id_2_4 =>
        DailyRecordsSetsListAprilNr2().Where(r => r.Id <= 4).ToList();
    public static List<DailyRecordsSet> DailyRecordsSetsListAprilNr2Id_4_5 =>
        DailyRecordsSetsListAprilNr2().Where(r => r.Id > 3 && r.Id <= 5).ToList();
    public static List<DailyRecordsSet> DailyRecordsSetsListAprilNr2Id_3_5 =>
        DailyRecordsSetsListAprilNr2().Where(r => r.Id > 2 && r.Id <= 5).ToList();
    public static List<Domain.Entity.Record> CombinedListsFromNr6ToNr9 =>
        RecordList01April2023TagsNr6().Concat(RecordList02April2023TagsNr7()).Concat(RecordList03April2023TagsNr8()).Concat(RecordList04April2023TagsNr9()).ToList();
    public static List<Domain.Entity.Record> CombinedListsFromNr7ToNr9 =>
        RecordList02April2023TagsNr7().Concat(RecordList03April2023TagsNr8()).Concat(RecordList04April2023TagsNr9()).ToList();
    //*******************************************************************************************************
    public static List<Tag> TagsMerged_Nr06_Nr07()
    {
        List<Tag> list = new List<Tag>();

        for (int i = 0; i < 4; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 9,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[33 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[33 + i].Item2,
                ContentDate = tupleArray[33 + i].Item1,
                IdOfInitialRecord = i + 1 + 6,
                Name = subTagsArrayID04_11[i],
                NameOfInitialRecord = "Rec_From_01April_list6"
            };
            list.Add(tag);
        }
        Tag tag5 = new Tag
        {
            Id = 15,
            CreatedById = 2,
            CreatedDateTime = tupleArray[38].Item2,
            ModifiedById = 2,
            ModifiedDateTime = tupleArray[38].Item2,
            ContentDate = tupleArray[38].Item1,
            IdOfInitialRecord = 9,
            Name = subTagsArrayID04_11[4],
            NameOfInitialRecord = "Rec_From_02April_list7"
        };
        list.Add(tag5);
        return list;
    }
    public static List<Tag> TagsMerged_Nr05_Nr08()
    {
        List<Tag> list = new List<Tag>();

        Tag tag0 = new Tag
        {
            Id = 0 + 1 + 5,
            CreatedById = 0 + 1,
            CreatedDateTime = tupleArray[29 + 0].Item2,
            ModifiedById = 0 + 1,
            ModifiedDateTime = tupleArray[29 + 0].Item2,
            ContentDate = tupleArray[29 + 0].Item1,
            IdOfInitialRecord = 0 + 1 + 5,
            Name = subTagsArrayID00_03ID12_15[0],
            NameOfInitialRecord = "Rec_From_05march_list5",
            TimeTokens = 6
        };
        list.Add(tag0);

        Tag tag1 = new Tag
        {
            Id = 1 + 1 + 5,
            CreatedById = 1 + 1,
            CreatedDateTime = tupleArray[29 + 1].Item2,
            ModifiedById = 1 + 1,
            ModifiedDateTime = tupleArray[29 + 1].Item2,
            ContentDate = tupleArray[29 + 1].Item1,
            IdOfInitialRecord = 1 + 1 + 5,
            Name = subTagsArrayID00_03ID12_15[1],
            NameOfInitialRecord = "Rec_From_05march_list5",
            TimeTokens = 3
        };
        list.Add(tag1);

        Tag tag2 = new Tag
        {
            Id = 2 + 1 + 5,
            CreatedById = 2 + 1,
            CreatedDateTime = tupleArray[29 + 2].Item2,
            ModifiedById = 2 + 1,
            ModifiedDateTime = tupleArray[29 + 2].Item2,
            ContentDate = tupleArray[29 + 2].Item1,
            IdOfInitialRecord = 2 + 1 + 5,
            Name = subTagsArrayID00_03ID12_15[2],
            NameOfInitialRecord = "Rec_From_05march_list5",
            TimeTokens = 8
        };
        list.Add(tag2);

        for (int i = 0; i < 3; i++)
        {
            Tag tag = new Tag
            {
                Id = i + 1 + 17,
                CreatedById = i + 1,
                CreatedDateTime = tupleArray[41 + i].Item2,
                ModifiedById = i + 1,
                ModifiedDateTime = tupleArray[41 + i].Item2,
                ContentDate = tupleArray[41 + i].Item1,
                IdOfInitialRecord = i + 1 + 8,
                Name = subTagsArrayID00_03ID12_15[i + 3],
                NameOfInitialRecord = "Rec_From_03April_list8",
                TimeTokens = 5
            };
            list.Add(tag);
        }
        return list;
    }

    //*******************************************************************************************************
    private static string BuildContent3()
    {
        var buildContent3 = new StringBuilder();
        buildContent3.AppendLine("Przykładowy teskt");
        buildContent3.AppendLine();
        buildContent3.AppendLine("***");
        buildContent3.AppendLine();
        buildContent3.AppendLine("daohdoaishdoaihdoahd");
        buildContent3.AppendLine();
        buildContent3.AppendLine("***");
        buildContent3.AppendLine();
        return buildContent3.ToString();
    }
    private static string BuildContent2()
    {
       var buildContent2 = new StringBuilder();
        buildContent2.AppendLine("Przykładowy $5 tekst 3 #trzeci");
        buildContent2.AppendLine();
        buildContent2.AppendLine("***");
        buildContent2.AppendLine();
        buildContent2.AppendLine("Tekst 3 $5#tekst");
        buildContent2.AppendLine();
        buildContent2.AppendLine("***");
        buildContent2.AppendLine();
        buildContent2.AppendLine("$5Przykład 3 #przykład");
        buildContent2.AppendLine();
        buildContent2.AppendLine("***");
        buildContent2.AppendLine();
        buildContent2.AppendLine("Inny opis 1 #opis$5");
        buildContent2.AppendLine();
        buildContent2.AppendLine("***");
        buildContent2.AppendLine();
        return buildContent2.ToString();
    }
    private static string BuildContent1()
    {
        var buildContent1 = new StringBuilder();
        buildContent1.AppendLine("Przykładowy opis 2 #opis");
        buildContent1.AppendLine();
        buildContent1.AppendLine("***");
        buildContent1.AppendLine();
        buildContent1.AppendLine("Opis 3 #opis");
        buildContent1.AppendLine();
        buildContent1.AppendLine("***");
        buildContent1.AppendLine();
        buildContent1.AppendLine("Tekst opisowy 1 #pierwszy");
        buildContent1.AppendLine();
        buildContent1.AppendLine("***");
        buildContent1.AppendLine();
        buildContent1.AppendLine("Inny tekst 3 #trzeci");
        buildContent1.AppendLine();
        buildContent1.AppendLine("***");
        buildContent1.AppendLine();
        return buildContent1 .ToString();
    }

    public static List<DailyRecordsSet> TestDailySetList =>
        new List<DailyRecordsSet>
            {
                new DailyRecordsSet()
                {
                     Id = 2,
                    CreatedById = 123,
                    CreatedDateTime = new DateTimeOffset(2023, 4, 4, 9, 0, 0, TimeSpan.Zero),
                    ModifiedById = 123,
                    ModifiedDateTime = new DateTimeOffset(2023, 4, 4, 9, 0, 0, TimeSpan.Zero),
                    RefersToDate = new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero),
                    MergedTags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "trzeci",
                            Id = 1,
                        },
                        new Tag
                        {
                            Name = "pierwszy",
                            Id = 2,
                        },
                        new Tag
                        {
                            Name = "opis",
                            Id = 4,
                        }

                    },
                    MergedContents = BuildContent1(),
                    EmailHasBeenSent = false,
                    HasAnyRecords = true,
                    HasAnyTags = true
                },

                new DailyRecordsSet()
                {
                     Id = 1,
                    CreatedById = 123,
                    CreatedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    ModifiedById = 123,
                    ModifiedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    RefersToDate = new DateTimeOffset(2023, 5, 11, 0, 0, 0, TimeSpan.Zero),
                    MergedTags = new List<Tag>
                    {
                        new Tag
                        {
                            Name = "trzeci",
                            Id = 1,
                            TimeTokens = 5
                        },
                        new Tag
                        {
                            Name = "tekst",
                            Id = 2,
                            TimeTokens = 5
                        },
                        new Tag
                        {
                            Name = "opis",
                            Id = 4,
                            TimeTokens = 5
                        },
                        new Tag
                        {
                            Name = "przykład",
                            Id = 3,
                            TimeTokens = 5
                        }
                    },
                    MergedContents = BuildContent2(),
                    EmailHasBeenSent = false,
                    HasAnyRecords = true,
                    HasAnyTags = true
                },

                new DailyRecordsSet()
                {
                     Id = 3,
                    CreatedById = 123,
                    CreatedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    ModifiedById = 123,
                    ModifiedDateTime = new DateTimeOffset(2023, 5, 11, 9, 0, 0, TimeSpan.Zero),
                    RefersToDate = new DateTimeOffset(2023, 5, 11, 0, 0, 0, TimeSpan.Zero),
                    MergedContents = BuildContent3(),
                    EmailHasBeenSent = false,
                    HasAnyRecords = true,
                    HasAnyTags = false
                },

                new DailyRecordsSet()
            };  

    //*******************************************************************************************************

}
