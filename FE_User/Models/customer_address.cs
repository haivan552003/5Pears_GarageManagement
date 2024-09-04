using System;

namespace FE_User.Models
{
    public class customer_address
    {
        public int id_address { get; set; }
        public string address { get; set; }
        public string type { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_cus { get; set; }
    }
}
