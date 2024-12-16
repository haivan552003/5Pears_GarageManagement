namespace FE_Admin.ModelCustom
{
    public class car_seat_not_null
    {
        public int id { get; set; }
    }
    public class CarRentalRequest
    {
        public DateTime DateStart { get; set; }
        public int CarId { get; set; }
    }
}
