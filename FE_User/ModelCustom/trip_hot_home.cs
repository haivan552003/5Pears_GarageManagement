using System;

namespace FE_User.Models
{
    public class trip_hot_home
    {
        public int id_trip { get; set; }
        public string img_trip { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string distance { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public byte is_delete { get; set; }
        public int emp_create { get; set; }
         public int id_trip_detail { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public int id_car { get; set; }
        public int id_location { get; set; }
        public int id_driver { get; set; }
    }
}
