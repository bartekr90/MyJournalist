namespace MyJournalist.App.Abstract;

public interface IEntityService<T>
{
    public List<T> Items { get; }
    List<T> GetAllItems();
    void SetItems(List<T> list);
    string GetFileName();
    T GetEmptyObj(IDateTimeProvider provider);
}
