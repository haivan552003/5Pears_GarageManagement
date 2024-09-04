using System;

namespace BE_API.Models
{
    public class car_seat
    {
        public int id_car_seat { get; set; }
        public string name { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_car { get; set; }
    }
}
