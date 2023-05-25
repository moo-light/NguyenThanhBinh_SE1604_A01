using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class FlowerBouquet:BaseEntity
{
    public int FlowerBouquetId { get; set; }

    public int CategoryId { get; set; }

    public string FlowerBouquetName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public byte? FlowerBouquetStatus { get; set; }

    public int? SupplierId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Supplier? Supplier { get; set; }
}
