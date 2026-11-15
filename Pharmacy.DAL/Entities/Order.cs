using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class Order
{
    public long Id { get; set; }

    public Guid CustomerId { get; set; }

    public Guid CheckoutKey { get; set; }

    public Guid? DriverId { get; set; }

    public string OrderType { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string UnavailablePolicy { get; set; } = null!;

    public int ZoneId { get; set; }

    public string ShippingZoneAr { get; set; } = null!;

    public string ShippingZoneEn { get; set; } = null!;

    public string ShippingName { get; set; } = null!;

    public string ShippingPhone { get; set; } = null!;

    public string ShippingGovernorate { get; set; } = null!;

    public string ShippingCity { get; set; } = null!;

    public string? ShippingDistrict { get; set; }

    public string ShippingStreet { get; set; } = null!;

    public string ShippingBuildingNumber { get; set; } = null!;

    public string? ShippingFloor { get; set; }

    public string? ShippingApartment { get; set; }

    public string? ShippingLandmark { get; set; }

    public string? ShippingDirections { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public string ZoneReview { get; set; } = null!;

    public long? CouponId { get; set; }

    public string? CouponCodeSnapshot { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? DirectDiscount { get; set; }

    public decimal? CouponDiscount { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal? Total { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? ConfirmedAt { get; set; }

    public string? CancellationReason { get; set; }

    public byte[] Version { get; set; } = null!;

    public virtual Coupon Coupon { get; set; } = null!;

    public virtual ICollection<CouponUsage> CouponUsages { get; set; } = new List<CouponUsage>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


    public virtual OrderStatusHistory? OrderStatusHistory { get; set; }

    public virtual ICollection<PrescriptionImage> PrescriptionImages { get; set; } = new List<PrescriptionImage>();

    public virtual ICollection<ProductRequest> ProductRequests { get; set; } = new List<ProductRequest>();

    public virtual DeliveryZone Zone { get; set; } = null!;
}
