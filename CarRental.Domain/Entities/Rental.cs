namespace CarRental.Domain.Entities;

public class Rental
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid CarId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsCanceled { get; private set; }

    private Rental() { }

    public Rental(Guid customerId, Guid carId, DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException("End date must be after start date");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        CarId = carId;
        StartDate = start;
        EndDate = end;
    }

    public void ModifyDates(DateTime newStartDate, DateTime newEndDate)
    {
        if (newStartDate >= newEndDate)
            throw new ArgumentException("Start date must be before end date.");

        StartDate = newStartDate;
        EndDate = newEndDate;
    }

    public void ChangeCar(Guid newCarId)
    {
        if (newCarId == Guid.Empty)
            throw new ArgumentException("Invalid car ID.");

        CarId = newCarId;
    }

    public void Cancel()
    {
        if (IsCanceled) throw new InvalidOperationException("Rental already canceled.");
        IsCanceled = true;
    }
}
