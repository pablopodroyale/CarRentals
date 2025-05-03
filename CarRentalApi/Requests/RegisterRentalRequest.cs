namespace CarRentalSolution.Requests
{
    public class RegisterRentalRequest
    {
        public Guid CustomerId { get; set; }
        public string CarType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
