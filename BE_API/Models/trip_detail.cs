using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;

namespace BE_API.Models
{
    public class trip_detail
    {
        public int id_trip_detail { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_trip { get; set; }
        public int id_car { get; set; }
        public int id_location { get; set; }
        public int id_driver { get; set; }
    }
}
