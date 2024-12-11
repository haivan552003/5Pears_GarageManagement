﻿namespace FE_Admin.ModelCustom
{
    public class TripCarLocation
    {
        public int id { get; set; }
        public string car_name { get; set; }
        public string car_code { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public float price { get; set; }
        public int type_id { get; set; }
        public string type_name { get; set; }
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public byte isAuto { get; set; }
        public DateTime year_production { get; set; }
        public float odo { get; set; }
        public float insurance_fee { get; set; }
        public int fuel { get; set; }
        public string description { get; set; }
        public int number_seat { get; set; }
        public List<trip_detail_custom> Trip_Detail_Customs { get; set; }
        public List<guest_cars_custom> guest_Cars_Customs { get; set; }
        public List<guest_car_driver_custom> guest_Car_Driver_Customs { get; set; }
    }

    public class trip_detail_custom
    {
        public string from { get; set; }
        public string to { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public string driver_name { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
    }

    public class trip_custom
    {
        public string from { get; set; }
        public string to { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
    }

    public class guest_cars_custom
    {
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }
    public class guest_car_driver_custom
    {
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public string driver_name { get; set; }
    }

    public class TripDetailViewModel
    {
        public string from { get; set; }
        public string to { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
    }
}
