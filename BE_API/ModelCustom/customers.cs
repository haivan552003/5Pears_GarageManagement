using System;

namespace BE_API.ModelCustom
{
    public class customers
    {
        public int Id { get; set; }
        public string cus_code { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public DateTime birthday { get; set; }
        public byte gender { get; set; }
        public string phone_number { get; set; }
        public string citizen_identity_img1 { get; set; }
        public string citizen_identity_number { get; set; }
        public string driver_license_img1 { get; set; }
        public string driver_license_number { get; set; }
        public int id_role { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
    }
}
