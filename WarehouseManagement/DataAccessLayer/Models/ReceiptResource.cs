using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class ReceiptResource
{
    public int Id { get; set; }

    public int DocumentId { get; set; }

    public int ResourceId { get; set; }

    public int UnitId { get; set; }

    public decimal Count { get; set; }

    public virtual ReceiptDocument Document { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;

    public virtual UnitMeasurement Unit { get; set; } = null!;
}
