﻿namespace MyJournalist.Domain.Common;

public class BaseEntity : AuditableModel
{
    public int Id { get; set; }
}
