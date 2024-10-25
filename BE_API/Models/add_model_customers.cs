using System;

namespace BE_API.Models
{
    public class add_model_customers
    {
        public string email { get; set; }
        public string pass_word { get; set; }
        public string full_name { get; set; }
        public DateTime birthday { get; set; }
        public byte gender { get; set; }
        public string phone_number { get; set; }
        public int id_role { get; set; }
    }
}
