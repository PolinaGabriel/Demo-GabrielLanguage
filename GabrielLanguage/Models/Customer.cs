using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class Customer
{
    public int Id { get; set; }

    public int SexId { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Patronym { get; set; } = null!;

    public DateOnly Birthdate { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly Firstdate { get; set; }

    public DateOnly? Lastdate { get; set; }

    public string Photo { get; set; } = null!;

    public int? Visits { get; set; }

    public virtual ICollection<CustomerTag> CustomerTags { get; set; } = new List<CustomerTag>();

    public virtual Sex Sex { get; set; } = null!;

    public virtual ICollection<Visit> VisitsNavigation { get; set; } = new List<Visit>();
}
