namespace Data.Models;

public record Account
{
    public Guid? Id { get; set; }

    public decimal Balance { get; set; }

    public required string Owner { get; set; }

    public DateTime Created { get; set; }

    public DateTime Modified { get; set; }

    public bool IsActive { get; set; }
}
