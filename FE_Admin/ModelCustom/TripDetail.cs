namespace FE_Admin.ModelCustom
{
    public class TripDetail
    {
        public int trip_detail_id { get; set; }
        public string trip_detail_code { get; set; }
        public string location_from_id { get; set; }
        public string location_to_id { get; set; }
        // public string car_code { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public bool status { get; set; }
    }
    public class Car
    {
        public int id { get; set; }
        public string car_name { get; set; }
        public string car_code { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public string status { get; set; }
        public byte isAuto { get; set; }
        public byte isRetail { get; set; }
        public float price { get; set; }
        public int type_id { get; set; }
        public int brand_id { get; set; }
        public string location_car { get; set; }
        public DateTime year_production { get; set; }
        public float odo { get; set; }
        public float insurance_fee { get; set; }
        public int fuel { get; set; }
        public string description { get; set; }
        public List<TripDetail> trip_Details { get; set; }
    }
}
