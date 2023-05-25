using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace BusinessObject;

public partial class Customer : BaseEntity
{
    public int CustomerId { get; set; }

    public string Email { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? Birthday { get; set; }
    [NotMapped]
    public bool IsAdmin { get; set; } = false;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string? ToString()
    {
        return $"{CustomerId} {Email} {CustomerName} {City} {Country} {Birthday}";
    }

    public Customer Clone()
    {
        return new Customer
        {
            Email = Email,
            CustomerName = CustomerName,
            City = City,
            Country = Country,
            Password = Password,
            Birthday = Birthday,
        };
    }
}
