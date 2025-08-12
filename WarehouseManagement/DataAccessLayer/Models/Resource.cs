using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Archive { get; set; }

    public virtual ICollection<Balance> Balances { get; set; } = new List<Balance>();

    public virtual ICollection<ReceiptResource> ReceiptResources { get; set; } = new List<ReceiptResource>();

    public virtual ICollection<SnipmentResource> SnipmentResources { get; set; } = new List<SnipmentResource>();
}
