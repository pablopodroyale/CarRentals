    namespace CarRental.Domain.Entities;

    public class Rental
    {
        public Guid Id { get; private set; }
        public Customer Customer { get; private set; }
        public Car Car { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsCanceled { get; private set; }

        private Rental() { }

        public Rental(Customer customer, Car car, DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End date must be after start date");

            Id = Guid.NewGuid();
            Customer = customer;
            Car = car;
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

        public void ChangeCar(Car newCar)
        {
            if (newCar == null)
                throw new ArgumentException("Invalid car");

            Car = newCar;
        }

        public void Cancel()
        {
            if (IsCanceled) throw new InvalidOperationException("Rental already canceled.");
            IsCanceled = true;
        }

        public static Rental Rehydrate(Guid id, Customer customer, Car car, DateTime start, DateTime end, bool isCanceled = false)
        {
            var rental = new Rental
            {
                Id = id,
                Customer = customer,
                Car = car,
                StartDate = start,
                EndDate = end,
                IsCanceled = isCanceled
            };

            return rental;
        }
}
