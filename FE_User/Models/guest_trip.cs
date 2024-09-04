using System;

namespace FE_User.Models
{
    public class guest_trip
    {
        public int id_guest_trip { get; set; }
        public byte is_delete { get; set; }
        public string status { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_guest { get; set; }
        public int id_emp { get; set; }
        public int id_cus { get; set; }
        public int id_trip_detail { get; set; }
    }
}
