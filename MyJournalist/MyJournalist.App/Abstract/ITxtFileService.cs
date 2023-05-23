namespace MyJournalist.App.Abstract;

public interface ITxtFileService
{
    void ClearData();
    DateTimeOffset GetLastWriteTime();
    string ReadData();
}
