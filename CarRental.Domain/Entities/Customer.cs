namespace CarRental.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string ApplicationUserId { get; set; }
    public Address Address { get; set; }
    public string FullName { get; set; }

}
