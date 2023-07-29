namespace Data.Models;

public record Payment
{
    public Guid? Id { get; set; }

    public Direction Direction { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public required string OtherEntity { get; set; }

    public bool IsActive { get; set; }

    public Guid AccountId { get; set; }
}