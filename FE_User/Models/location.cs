using System;

namespace FE_User.Models
{
    public class location
    {
        public int id_location { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone_number { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public byte is_delete { get; set; }
    }
}
