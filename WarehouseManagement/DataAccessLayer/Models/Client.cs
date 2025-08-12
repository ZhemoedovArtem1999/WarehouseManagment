using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class Client
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public bool Archive { get; set; }

    public virtual ICollection<SnipmentDocument> SnipmentDocuments { get; set; } = new List<SnipmentDocument>();
}
