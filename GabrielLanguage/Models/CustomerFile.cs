using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class CustomerFile
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int FileId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual File File { get; set; } = null!;
}
