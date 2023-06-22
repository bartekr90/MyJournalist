namespace MyJournalist.App.Abstract
{
    public interface IFileConfig
    {
        string JsonFileLocation { get; set; }
        string TxtFileLocation { get; set; }
        string TxtName { get; set; }
    }
}