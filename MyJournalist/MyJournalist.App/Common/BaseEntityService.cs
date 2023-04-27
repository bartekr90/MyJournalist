using MyJournalist.App.Abstract;
using MyJournalist.Domain.Common;


namespace MyJournalist.App.Common;

public class BaseEntityService<T> : IEntityService<T> where T : BaseEntity, new()
{
    public List<T> Items { get; private set; }

    public BaseEntityService()
    {
        Items = new List<T>();
    }

    public List<T> GetAllItems()
    {
        return Items;
    }

    public void SetItems(List<T> list)
    {
        Items = list;
    }

    public virtual string GetFileName()
    {
        return $"{typeof(T).Name}s";
    }

    public virtual T GetEmptyObj(IDateTimeProvider provider)
    {
        return new T();
    }
}

