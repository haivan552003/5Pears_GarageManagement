using System;

namespace BE_API.Models
{
    public class CarSeat
    {
        public string SeatName { get; set; }
        public int row { get; set; }
        public int col { get; set; }
    }

    public class Lookup
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime TripStartDate { get; set; }
        public DateTime TripEndDate { get; set; }
        public decimal TripPrice { get; set; }
        public string TripStatus { get; set; }
        public string CarSeat { get; set; }
        public string CarNumber { get; set; }
        public string CarColor { get; set; }
        public string CarName { get; set; }
        public string CarType { get; set; }
        public string CarBrand { get; set; }
        public string DriverName { get; set; }
        public string TripFrom { get; set; }
        public string TripTo { get; set; }
        public string TicketCode { get; set; }
    }
}
