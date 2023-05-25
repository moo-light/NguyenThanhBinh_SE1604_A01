using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Order:BaseEntity
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public DateTime? ShippedDate { get; set; }

    public decimal? Total { get; set; }

    public string? OrderStatus { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
