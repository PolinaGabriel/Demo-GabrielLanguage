using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class Visit
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public int CustomerId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<VisitFile> VisitFiles { get; set; } = new List<VisitFile>();
}
