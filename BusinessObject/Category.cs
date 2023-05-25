using System;
using System.Collections.Generic;

namespace BusinessObject;

public partial class Category :BaseEntity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? CategoryDescription { get; set; }

    public virtual ICollection<FlowerBouquet> FlowerBouquets { get; set; } = new List<FlowerBouquet>();
}
