using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class Coupon
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string? NormalizedCode { get; set; }

    public string? Description { get; set; }

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public DateTime? StartsAt { get; set; }

    public DateTime? EndsAt { get; set; }

    public decimal MinimumSubtotal { get; set; }

    public decimal? MaximumDiscount { get; set; }

    public int? GlobalLimit { get; set; }

    public int? LimitPerCustomer { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
