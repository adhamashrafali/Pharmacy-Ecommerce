using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class CouponUsage
{
    public long OrderId { get; set; }

    public long CouponId { get; set; }

    public string State { get; set; } = null!;

    public decimal AppliedAmount { get; set; }

    public DateTime ReservedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
