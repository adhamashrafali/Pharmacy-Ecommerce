using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class CartItem
{
    public Guid CustomerId { get; set; }

    public long ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Cart Customer { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
