using System;
using System.Reflection;

namespace FE_User.Models
{
    public class driver
    {
        public int id_driver { get; set; }
        public string img_driver { get; set; }
        public string full_name { get; set; }
        public DateTime birthday { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public byte gender { get; set; }
        public string driver_license_img { get; set; }
        public string driver_license_number { get; set; }
        public string status { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
    }
}
