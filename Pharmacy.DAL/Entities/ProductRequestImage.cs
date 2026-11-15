using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class ProductRequestImage
{
    public long Id { get; set; }

    public long RequestId { get; set; }

    public string ObjectKey { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual ProductRequest Request { get; set; } = null!;
}
