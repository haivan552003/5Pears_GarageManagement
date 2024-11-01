using System;

namespace FE_User.Models
{
    public class trip_detail
    {
        public int id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public int trip_id { get; set; }
        public int car_id { get; set; }
        public int id_location { get; set; }
        public int id_driver { get; set; }
        public string trip_detail_code { get; set; }
        public float distance { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
    }
}
