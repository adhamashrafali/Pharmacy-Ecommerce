using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class Address
{
    public long Id { get; set; }

    public Guid CustomerId { get; set; }

    public int ZoneId { get; set; }

    public string Label { get; set; } = null!;

    public string Governorate { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? District { get; set; }

    public string Street { get; set; } = null!;

    public string BuildingNumber { get; set; } = null!;

    public string? Floor { get; set; }

    public string? Apartment { get; set; }

    public string? Landmark { get; set; }

    public string? Directions { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public bool IsDefault { get; set; }

    public virtual DeliveryZone Zone { get; set; } = null!;
}
