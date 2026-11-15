using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class ProductRequest
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Guid? ReviewedBy { get; set; }

    public DateTime? ReviewedAt { get; set; }

    public string? ReviewNotes { get; set; }

    public long? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual ICollection<ProductRequestImage> ProductRequestImages { get; set; } = new List<ProductRequestImage>();
}
