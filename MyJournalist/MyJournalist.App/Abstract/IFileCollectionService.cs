using MyJournalist.Domain.Common;

namespace MyJournalist.App.Abstract;

public interface IFileCollectionService<T> where T : BaseEntity
{
    void CreateFile(string fileName);
    ICollection<T> ReadFile(string fileName);
    void UpdateFile(ICollection<T> obj, string fileName);
    void ClearFile(string fileName);
    bool CheckFileExists(string fileName);
}
