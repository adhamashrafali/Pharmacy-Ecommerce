using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class WishlistItem
{
    public Guid CustomerId { get; set; }

    public long ProductId { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
