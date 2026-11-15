using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class ProductImage
{
    public long Id { get; set; }

    public long ProductId { get; set; }

    public string ObjectKey { get; set; } = null!;

    public int DisplayOrder { get; set; }

    public bool IsPrimary { get; set; }

    public virtual Product Product { get; set; } = null!;
}
