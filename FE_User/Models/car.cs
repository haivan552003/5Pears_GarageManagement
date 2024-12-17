using System;
using System.Drawing;

namespace FE_User.Models
{
    public class car
    {
        public int id { get; set; }
        public string car_name { get; set; }
        public string img_name { get; set; }

        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public string status { get; set; }
        public byte isAuto { get; set; }
        public byte is_retail { get; set; }
        public float price { get; set; }
        public int type_id { get; set; }
        public string type_name { get; set; }

        public int brand_id { get; set; }
        public string brand_name { get; set; }

        public DateTime year_production { get; set; }
        public float odo { get; set; }
        public float insurance_fee { get; set; }

        public int fuel {  get; set; }
        public string description {  get; set; }
        public float voucher {  get; set; }
        public int number_seat {  get; set; }
        public float price_user {  get; set; }
        public int rate {  get; set; }
        public decimal rate_avg {  get; set; }
        public List<car_img> car_img { get; set; }


    }
}
