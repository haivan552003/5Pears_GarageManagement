namespace FE_Admin.Models
{
    public class CarRentalRequest
    {
        public DateTime DateStart { get; set; }
        public int CarId { get; set; }
    }
    public class DriverRentalRequest
    {
        public DateTime DateStart { get; set; }
        public int DriverId { get; set; }
        public int CarID { get; set; }
    }
}
