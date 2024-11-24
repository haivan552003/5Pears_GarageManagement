namespace FE_User.ModelCustom
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
    }
}
