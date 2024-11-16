namespace FE_User.ModelCustom
{
    public class TRIPDETAILS
    {
        public int id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public int trip_id { get; set; }
        public int car_id { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public int location_from_id { get; set; }
        public string trip_detail_code { get; set; }
        public int driver_id { get; set; }
        public int location_to_id { get; set; }
        public float distance { get; set; }
        public bool status { get; set; }
        public string car_number { get; set; }
        public string car_code { get; set; }
        public string car_name { get; set; }
        public string fullname { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public float sale { get; set; }
        public List<car_seat> car_seat { get; set; }
    }

    public class car_seat
    {
        public int id { get; set; }
        public string name { get; set; }
        public int car_id { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public string status { get; set; }
    }
}
