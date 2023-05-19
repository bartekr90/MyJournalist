using MyJournalist.Domain.Entity;

namespace MyJournalist.App.Tests;

internal static class SampleDataSet
{
    //*******************************************************************************************************

    public static Tuple<DateTimeOffset, DateTimeOffset>[] tupleArray = //48 elements
     {
        Tuple.Create(DateTimeOffset.Now,DateTimeOffset.Now + TimeSpan.FromMinutes(+1)),
        Tuple.Create(DateTimeOffset.MinValue,DateTimeOffset.MinValue),
        Tuple.Create(DateTimeOffset.MaxValue,DateTimeOffset.MaxValue),
        Tuple.Create(new DateTimeOffset(),new DateTimeOffset(2001, 1, 7, 10, 30, 0, TimeSpan.Zero)),
        Tuple.Create(DateTimeOffset.MinValue,new DateTimeOffset(9999, 12, 31, 23, 59, 59, TimeSpan.Zero)),
        Tuple.Create(new DateTimeOffset(2023, 5, 7, 23, 59, 59, TimeSpan.Zero),new DateTimeOffset(2023, 5, 9, 10, 0, 0, TimeSpan.Zero)),
        Tuple.Create(DateTimeOffset.Now + TimeSpan.FromSeconds(-59),DateTimeOffset.Now),
        Tuple.Create(new DateTimeOffset(2023, 4, 1, 12, 0, 0, TimeSpan.Zero),new DateTimeOffset(2023, 5, 11, 18, 30, 0, TimeSpan.Zero)),
        Tuple.Create(new DateTimeOffset(2022, 8, 15, 6, 0, 0, TimeSpan.Zero),new DateTimeOffset(2022, 8, 15, 8, 45, 0, TimeSpan.Zero)),
        Tuple.Create(new DateTimeOffset(2023, 1, 1, 0, 0, 0, TimeSpan.Zero),new DateTimeOffset(2023, 1, 2, 23, 59, 59, TimeSpan.Zero)),
        Tuple.Create(new DateTimeOffset(2022, 6, 10, 10, 0, 0, TimeSpan.Zero),new DateTimeOffset(2022, 6, 10, 18, 0, 0, TimeSpan.Zero)),
        Tuple.Create(new DateTimeOffset(2023, 12, 1, 0, 0, 0, TimeSpan.Zero),new DateTimeOffset(2024, 2, 28, 23, 59, 59, TimeSpan.Zero)),
        Tuple.Create(new DateTimeOffset(2021, 12, 1, 0, 0, 0, TimeSpan.Zero),new DateTimeOffset(2024, 2, 28, 23, 59, 59, TimeSpan.Zero)),
  /*13*/Tuple.Create(new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 3, 1, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*17*/Tuple.Create(new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 3, 2, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*21*/Tuple.Create(new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 3, 3, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*25*/Tuple.Create(new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 3, 4, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*29*/Tuple.Create(new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 3, 5, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*33*/Tuple.Create(new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 4, 1, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*37*/Tuple.Create(new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 4, 2, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*41*/Tuple.Create(new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 4, 3, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  /*45*/Tuple.Create(new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(1),new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(2)),
        Tuple.Create(new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(3),new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(4)),
        Tuple.Create(new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(5),new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(6)),
        Tuple.Create(new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(7),new DateTimeOffset(2023, 4, 4, 0, 0, 0, TimeSpan.Zero).AddHours(8)),
  };

    //*******************************************************************************************************

    public static string[] contentArray =
    {//31 elem
        "Przykładowy tekst 1",
        "To jest drugi przykład",
        "Witaj w świecie programowania",
        "Lorem ipsum dolor sit amet",
        "Testowy tekst numer 5",
        "Cześć, jak się masz?",
        "To jest szósty element",
        "Sprawdź tę listę tekstów",
        "Przykład numer 8",
        "Ostatni tekst w liście",
        "Tekst numer 11",
        "Jakiś inny przykładowy tekst",
        "Kolejny tekst w linii",
        "To jest czternasty element",
        "Lorem ipsum dolor sit amet consectetur",
        "Tekst numer 16",
        "Cześć, co u ciebie słychać?",
        "To jest osiemnasty przykład",
        "Sprawdź tę kolekcję tekstów",
        "Przykład numer 20",
        "Ostatni tekst w kolekcji",
        "Tekst numer 22",
        "Inny przykładowy tekst",
        "Jeszcze jeden tekst",
        "To jest dwudziesty piąty element",
        "Lorem ipsum dolor sit",
        "Tekst numer 27",
        "Cześć, jak minął ci dzień?",
        "To jest dwudziesty siódmy przykład",
        "Sprawdź tę listę przykładów",
        "Przykład numer 30"
    };

    //*******************************************************************************************************

    public static string[] contentArrayWithTags =
     {//31
        "Przykładowy tekst 2 $3 #drugi",
        "Inny tekst 1 $3 #pierwszy",
        "Losowy opis 1 $3 #opis",
        "Tekst numer 2 $3 #drugi",
        "Przykładowy opis 1 #opis $12",
        "Inny tekst 2 #drugi",
        "Opisowy tekst 1 #pierwszy",
        "Przykład 2 #przykład",
        "Tekst przykładowy 1 #przykład",
        "Tekst 2 #tekst",
        "Inny przykład 1 #przykład",
        "Opis 2 #opis",
        "Przykładowy $5 tekst 3 #trzeci",
        "Tekst 3 $5#tekst",
        "$5Przykład 3 #przykład",
        "Inny opis 1 #opis$5",
        "Przykładowy opis 2 #opis",
        "Opis 3 #opis",
        "Tekst opisowy 1 #pierwszy",
        "Inny tekst 3 #trzeci",
        "Przykład 4 #przykład",
        "Tekst 4 #tekst",
        "Opis przykładowy 1 #opis",
        "Przykładowy tekst 4 #czwarty",
        "Inny opis 2 #opis",
        "Tekst 5 #tekst",
        "Opis 4 #opis",
        "Przykład 5 #przykład",
        "Przykładowy opis 3 #opis",
        "Tekst opisowy 2 #pierwszy",
        "Inny tekst 4 #czwarty"
    };
    public static string MergedContentArrayWithTagsId0_3 =>
       $"{contentArrayWithTags[0]} {contentArrayWithTags[1]} {contentArrayWithTags[2]} {contentArrayWithTags[3]}";

    public static string MergedContentArrayWithTagsId4_7 =>
       $"{contentArrayWithTags[4]} {contentArrayWithTags[5]} {contentArrayWithTags[6]} {contentArrayWithTags[7]}";

    public static string MergedContentArrayWithTagsId8_11 =>
       $"{contentArrayWithTags[8]} {contentArrayWithTags[9]} {contentArrayWithTags[10]} {contentArrayWithTags[11]}";

    //*******************************************************************************************************

    public static string[] tagsArray =
    {//31
        "drugi",
        "pierwszy",
        "opis",
        "drugi",
        "opis",
        "drugi",
        "pierwszy",
        "przykład",
        "przykład",
        "tekst",
        "przykład",
        "opis",
        "trzeci",
        "tekst",
        "przykład",
        "opis",
        "opis",
        "opis",
        "pierwszy",
        "trzeci",
        "przykład",
        "tekst",
        "opis",
        "czwarty",
        "opis",
        "tekst",
        "opis",
        "przykład",
        "opis",
        "pierwszy",
        "czwarty"
    };

    public static string[] subTagsArrayId0_3 =>
        new string[] { tagsArray[0], tagsArray[1], tagsArray[2], tagsArray[3] };

    public static string[] subTagsArrayId4_7 =>
        new string[] { tagsArray[4], tagsArray[5], tagsArray[6], tagsArray[7] };

    public static string[] subTagsArrayId8_11 =>
        new string[] { tagsArray[8], tagsArray[9], tagsArray[10], tagsArray[11] };

    //*******************************************************************************************************
    public static string[] subTagsArrayID04_11 =>
       new string[] { tagsArray[4], tagsArray[5], tagsArray[6], tagsArray[7], tagsArray[9] };

    public static string[] subTagsArrayID00_03ID12_15 =>
          new string[] { tagsArray[0], tagsArray[1], tagsArray[2], tagsArray[12], tagsArray[13], tagsArray[14] };

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

}
