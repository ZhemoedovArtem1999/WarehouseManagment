using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class ReceiptDocument
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public DateOnly Date { get; set; }

    public virtual ICollection<ReceiptResource> ReceiptResources { get; set; } = new List<ReceiptResource>();
}
