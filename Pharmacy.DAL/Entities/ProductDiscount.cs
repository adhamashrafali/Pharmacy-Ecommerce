using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class ProductDiscount
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string DiscountType { get; set; } = null!;

    public decimal DiscountValue { get; set; }

    public DateTime StartsAt { get; set; }

    public DateTime EndsAt { get; set; }

    public int MinQuantity { get; set; }

    public bool IsActive { get; set; }

    public virtual Product Product { get; set; } = null!;
}
