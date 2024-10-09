using System;
using System.Reflection;

namespace BE_API.Models
{
    public class driver
    {
        public int id { get; set; }
        public string img_driver { get; set; }
        public string fullname { get; set; }
        public float price { get; set; }
        public float voucher {  get; set; }
        public DateTime birthday { get; set; }
        public string citizen_identity_img1 { get; set; }
        public string citizen_identity_img2 { get; set; }
        public string citizen_identity_number { get; set; }
        public string driver_license_img1 { get; set; }
        public string driver_license_img2 { get; set; }
        public string driver_license_number { get; set; }
        public byte gender { get; set; }
        public string phonenumber { get; set; }
        public string address { get; set; }
        public byte status { get; set; }
     
    }
}
