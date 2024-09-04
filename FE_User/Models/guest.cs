using System;

namespace FE_User.Models
{
    public class guest
    {
        public int id_guest { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string address { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public string driver_license_img { get; set; }
        public string driver_license_number { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_emp { get; set; }
    }
}
