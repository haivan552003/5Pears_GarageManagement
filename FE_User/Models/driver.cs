using System;
using System.Reflection;

namespace FE_User.Models
{
    public class driver
    {
        public int id { get; set; }
        public string img_driver { get; set; }
        public string fullname { get; set; }
        public DateTime birthday { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public byte gender { get; set; }
        public string driver_license_img { get; set; }
        public string driver_license_number { get; set; }
        public byte status { get; set; }
        public string phonenumber { get; set; }
        public byte is_delete { get; set; }
        public int age { get; set; }
        public float price { get; set; }
        public int class_driver_license { get; set; }
    }
}
