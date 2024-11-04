using System;

namespace FE_User.Models
{
    public class customer_address
    {
        public int id { get; set; }
        public string address { get; set; }
        public int id_cus { get; set; }
        public byte type { get; set; }
        public bool status { get; set; }
    }
}
