using System;

namespace BE_API.Models
{
    public class lookup
    {
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string CitizenID { get; set; }
        public DateTime TripStartDate { get; set; }
        public DateTime TripEndDate { get; set; }
        public decimal TripPrice { get; set; }
        public string TripStatus { get; set; }
        public string CarSeat { get; set; }
        public string CarNumber { get; set; }
        public string CarType { get; set; }
        public string CarBrand { get; set; }
        public string DriverName { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string TripFrom { get; set; }
        public string TripTo { get; set; }
        public string TripDistance { get; set; }
        public int TicketCode { get; set; } 
    }
}
