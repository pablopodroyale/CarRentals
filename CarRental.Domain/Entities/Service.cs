namespace CarRental.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public DateTime Date { get; set; }
}
