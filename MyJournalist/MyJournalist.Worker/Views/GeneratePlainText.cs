using MyJournalist.Domain.Entity;
using System.Text;

namespace TestWorker.Views;

public static class GeneratePlainText
{
    public static string GetDailyRecordsSetBody(DailyRecordsSet model)
    {
        uint tokens = 0;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Podsumowanie wpisów z dnia: ");
        sb.AppendLine($"{model.RefersToDate.Date.ToString("d")}");
        sb.AppendLine("");
        sb.AppendLine("Zawartość: ");
        sb.AppendLine($"{model.MergedContents}");
        sb.AppendLine("");

        if (model.HasAnyTags)
        {
            sb.AppendLine("Tagi: ");

            foreach (var tag in model.MergedTags)
            {
                tokens += tag.TimeTokens;
                sb.Append($"#{tag.Name} ");
            }
            sb.AppendLine("");
            sb.AppendLine("");
        }
        else
        {
            sb.AppendLine("Wpisów nie oznaczono tagami");
            sb.AppendLine("");
        }

        if (model.HasAnyTags && tokens > 0)
        {
            sb.AppendLine("Poświęcony czas: ");

            foreach (var tag in model.MergedTags)
            {
                if (tag.TimeTokens != 0)
                {
                    sb.Append($"#{tag.Name} ");
                    sb.Append("wykonywano przez: ");
                    sb.Append($"{tag.TimeTokens} ");
                    sb.AppendLine("minut.");
                }
            }
        }
        sb.AppendLine("");
        sb.AppendLine("Ten mail został wygenerowany automatycznie przez aplikację MyJournalist.");

        return sb.ToString();
    }
}
