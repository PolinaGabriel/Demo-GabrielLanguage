using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class VisitFile
{
    public int Id { get; set; }

    public int VisitId { get; set; }

    public int FileId { get; set; }

    public virtual File File { get; set; } = null!;

    public virtual Visit Visit { get; set; } = null!;
}
