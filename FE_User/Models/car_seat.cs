using System;

namespace FE_User.Models
{
    public class car_seat
    {
        public int id { get; set; }
        public string name { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int car_id { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public int status { get; set; }
    }
}
