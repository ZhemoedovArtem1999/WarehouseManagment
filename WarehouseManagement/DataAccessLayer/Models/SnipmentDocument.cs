using System;
using System.Collections.Generic;

namespace DataAccessLayer.Models;

public partial class SnipmentDocument
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public int ClientId { get; set; }

    public DateOnly Date { get; set; }

    public bool Sign { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<SnipmentResource> SnipmentResources { get; set; } = new List<SnipmentResource>();
}
