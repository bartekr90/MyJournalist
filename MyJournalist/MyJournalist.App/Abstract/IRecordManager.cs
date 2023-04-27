namespace MyJournalist.App.Abstract;

public interface IRecordManager
{
    void ClearTxt();
    List<Record> FindEqualDateRecords(ICollection<Record> list, DateTimeOffset compareDate);
    List<Record> FindPastDateRecords(ICollection<Record> list, DateTimeOffset compareDate);
    List<Record> GeRecordsWithContent(List<Record> group);
    string GetDataFromTxt();
    Record GetRecord(string contentFromTxt, ICollection<Tag> tags);
    IEnumerable<IGrouping<DateTime, Record>> GroupRecordsByDate(ICollection<Record> records);
    void LoadRecordsFromFile();
    List<Record> MakeNewRecordList(Record record);
    void SaveListInFile(List<Record> listToSave);
    void SaveRecordInFile(Record record);
}
