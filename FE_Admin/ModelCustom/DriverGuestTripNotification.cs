namespace FE_Admin.ModelCustom
{
    public class DriverGuestTripNotification
    {
        public string trip_detail_code { get; set; }
        public string location_name { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
    }
    public class DriverGuestNotification
    {
        public string guest_driver_code { get; set; }
        public string date { get; set; }
    }


    public class DriverGuestCarNotification
    {
        public string guest_car_code { get; set; }
        public string date { get; set; }
        public string car_name { get; set; }
    }

}

