using MyJournalist.Domain.Entity;
using System.Text;

namespace MyJournalist.Worker.Views;

public static class GeneratePlainText
{
    public static string GetDailyRecordsSetBody(DailyRecordsSet model)
    {
        uint tokens = 0;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Summary of the day's entries: ");
        sb.AppendLine($"{model.RefersToDate.Date.ToString("d")}");
        sb.AppendLine("");
        sb.AppendLine("Content: ");
        sb.AppendLine($"{model.MergedContents}");
        sb.AppendLine("");

        if (model.HasAnyTags)
        {
            sb.AppendLine("Tags: ");

            foreach (var tag in model.MergedTags!)
            {
                tokens += tag.TimeTokens;
                sb.Append($"#{tag.Name} ");
            }
            sb.AppendLine("");
            sb.AppendLine("");
        }
        else
        {
            sb.AppendLine("The following entries were not tagged with.");
            sb.AppendLine("");
        }

        if (model.HasAnyTags && tokens > 0)
        {
            sb.AppendLine("Time commitment: ");

            foreach (var tag in model.MergedTags!)
            {
                if (tag.TimeTokens != 0)
                {
                    sb.Append($"#{tag.Name} ");
                    sb.Append("was performed by: ");
                    sb.Append($"{tag.TimeTokens} ");
                    sb.AppendLine("minutes.");
                }
            }
        }
        sb.AppendLine("");
        sb.AppendLine("This email was generated automatically by MyJournalist.");

        return sb.ToString();
    }

    internal static string GetRecordBody(Record model)
    {
        uint tokens = 0;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Entrie on demand");
        sb.AppendLine($"{model.Name}");
        sb.AppendLine($"{model.ContentDate.ToString("d")}");
        sb.AppendLine("");
        sb.AppendLine("Content: ");
        sb.AppendLine($"{model.Content}");
        sb.AppendLine("");

        if (model.HasAnyTags)
        {
            sb.AppendLine("Tags: ");

            foreach (var tag in model.Tags!)
            {
                tokens += tag.TimeTokens;
                sb.Append($"#{tag.Name} ");
            }
            sb.AppendLine("");
            sb.AppendLine("");
        }
        else
        {
            sb.AppendLine("The following entries were not tagged with.");
            sb.AppendLine("");
        }

        if (model.HasAnyTags && tokens > 0)
        {
            sb.AppendLine("Time commitment: ");

            foreach (var tag in model.Tags!)
            {
                if (tag.TimeTokens != 0)
                {
                    sb.Append($"#{tag.Name} ");
                    sb.Append("was performed by: ");
                    sb.Append($"{tag.TimeTokens} ");
                    sb.AppendLine("minutes.");
                }
            }
        }
        sb.AppendLine("");
        sb.AppendLine("This email was generated on demand by MyJournalist.");

        return sb.ToString();
    }
}
