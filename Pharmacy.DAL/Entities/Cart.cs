using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class Cart
{
    public Guid CustomerId { get; set; }

    public DateTime UpdatedAt { get; set; }

    public byte[] Version { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
