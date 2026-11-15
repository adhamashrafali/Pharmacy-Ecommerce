using System;
using System.Collections.Generic;

namespace Pharmacy.DAL.Entities;

public partial class CustomerOrderCount
{
    public Guid CustomerId { get; set; }

    public long? OrderCount { get; set; }
}
