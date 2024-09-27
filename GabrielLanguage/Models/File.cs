using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class File
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public virtual ICollection<VisitFile> VisitFiles { get; set; } = new List<VisitFile>();
}
