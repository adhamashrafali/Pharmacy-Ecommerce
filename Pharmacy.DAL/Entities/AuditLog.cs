using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class AuditLog
{
    public long Id { get; set; }

    public Guid? ActorId { get; set; }

    public string EntityType { get; set; } = null!;

    public string EntityId { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string? DetailsJson { get; set; }

    public DateTime OccurredAt { get; set; }
}
