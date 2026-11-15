using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class DriverOrder
{
    public Guid? DriverId { get; set; }

    public long OrderId { get; set; }

    public string Status { get; set; } = null!;

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

    public decimal? CodAmount { get; set; }
}
