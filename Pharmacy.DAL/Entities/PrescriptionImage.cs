using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class PrescriptionImage
{
    public long Id { get; set; }

    public long OrderId { get; set; }

    public string ObjectKey { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual Order Order { get; set; } = null!;
}
