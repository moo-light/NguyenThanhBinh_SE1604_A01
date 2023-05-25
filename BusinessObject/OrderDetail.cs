using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject;

public partial class OrderDetail : BaseEntity
{
    public int OrderId { get; set; }

    public int FlowerBouquetId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double Discount { get; set; }

    public virtual FlowerBouquet FlowerBouquet { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
    [NotMapped]
    public decimal ActualPrice =>Convert.ToDecimal(Convert.ToDouble(UnitPrice) * (double)Quantity * (1 - Discount / 100) );
}
