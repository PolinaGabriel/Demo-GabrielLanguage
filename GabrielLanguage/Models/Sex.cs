using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class Sex
{
    public int Id { get; set; }

    public string SexName { get; set; } = null!;

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
