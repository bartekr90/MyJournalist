namespace MyJournalist.Domain.Common;

public class AuditableModel
{
    public int CreatedById { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public int? ModifiedById { get; set; }
    public DateTimeOffset? ModifiedDateTime { get; set; }

}