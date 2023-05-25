using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Supplier:BaseEntity
{
    public int SupplierId { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierAddress { get; set; }

    public string? Telephone { get; set; }

    public virtual ICollection<FlowerBouquet> FlowerBouquets { get; set; } = new List<FlowerBouquet>();
}
