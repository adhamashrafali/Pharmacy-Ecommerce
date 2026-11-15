using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class OrderItem
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public long ProductId { get; set; }

    public string NameAr { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal DirectDiscount { get; set; }

    public decimal CouponDiscount { get; set; }

    public decimal? LineTotal { get; set; }

    public string? ReplacementReason { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
