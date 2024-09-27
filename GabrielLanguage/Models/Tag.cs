using System;
using System.Collections.Generic;

namespace GabrielLanguage.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string TagName { get; set; } = null!;

    public string Color { get; set; } = null!;

    public virtual ICollection<CustomerTag> CustomerTags { get; set; } = new List<CustomerTag>();
}
