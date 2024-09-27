namespace GabrielLanguage.Models;

public partial class CustomerTag
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int TagId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}
