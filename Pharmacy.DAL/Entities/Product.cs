using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class Product
{
    public long Id { get; set; }

    public int? ManufacturerId { get; set; }

    public int? BrandId { get; set; }

    public string NameAr { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public string? DescriptionAr { get; set; }

    public string? DescriptionEn { get; set; }

    public string ProductType { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public bool IsAvailable { get; set; }

    public bool RequiresPrescription { get; set; }

    public int? MaxOrderQuantity { get; set; }

    public byte[] Version { get; set; } = null!;

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Manufacturer? Manufacturer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();

    public virtual ProductImage? ProductImage { get; set; }

    public virtual ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
