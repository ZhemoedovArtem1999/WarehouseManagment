using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Balance
{
    public int ResourceId { get; set; }

    public int UnitId { get; set; }

    public int Id { get; set; }

    public decimal Count { get; set; }

    public virtual Resource Resource { get; set; } = null!;

    public virtual UnitMeasurement Unit { get; set; } = null!;
}
