using System;
using System.IO;

namespace FE_User.Models
{
    public class customer
    {
        public int id_cus { get; set; }

        public string user_name { get; set; }
        public string pass_word { get; set; }
        public string full_name { get; set; }
        public DateTime birthday { get; set; }
        public byte gender { get; set; }
        public string phone_number { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public string driver_license_img { get; set; }
        public string driver_license_number { get; set; }
        public byte is_delete { get; set; }
        public int id_role { get; set; }
    }
}
