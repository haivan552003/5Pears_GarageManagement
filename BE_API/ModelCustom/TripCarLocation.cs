using System;
using System.Collections.Generic;

namespace BE_API.ModelCustom
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
        public bool isAuto { get; set; }
        public float price { get; set; }
        public int type_id { get; set; }
        public string type_name { get; set; }
        public int brand_id { get; set; }
        public string brand_name { get; set; }
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
        public string location { get; set; }
        public string driver_name { get; set; }
        public string date { get; set; }
    }
    public class guest_cars_custom
    {
        public string date { get; set; }
    }
    public class guest_car_driver_custom
    {
        public string date { get; set; }
        public string driver_name { get; set; }
    }
}
