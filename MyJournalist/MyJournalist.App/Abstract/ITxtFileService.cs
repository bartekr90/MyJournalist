namespace MyJournalist.App.Abstract;

public interface ITxtFileService
{
    void ClearData();
    DateTimeOffset GetGetLastWriteTime();
    string ReadData();
}
