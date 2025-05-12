namespace CarRental.Application.DTOs.Statistic
{
    public class MostRentedCarsDto
    {
        public string Type { get; set; }
        public string Model { get; set; }
        public double UtilizationPercentage { get; set; }
        public int TimesRented { get; set; } 

    }
}
