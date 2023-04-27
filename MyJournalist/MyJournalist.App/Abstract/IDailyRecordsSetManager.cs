using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyJournalist.Domain.Entity;


namespace MyJournalist.App.Abstract;

public interface IDailyRecordsSetManager
{
    DailyRecordsSet GetDailyRecordsSet(ICollection<Record>? records, ICollection<Tag>? tags);
    Dictionary<(int year, int month), List<DailyRecordsSet>> GroupDailySetsByMonth(List<DailyRecordsSet> dailySets);
    void LoadDailySetsFromFile(DateTimeOffset date);
    void SaveListInFile(ICollection<DailyRecordsSet> setsFromMonth);
    void SetDateForService(DateTimeOffset date);
}
