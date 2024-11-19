using System;
using System.Collections.Generic;
using System.Drawing;

namespace BE_API.Models
{
    public class car
    {
        public int id { get; set; }
        public string car_name { get; set; }
        public string car_code { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public string status { get; set; }
        public byte isAuto { get; set; }
        public byte isRetail { get; set; }
        public float price { get; set; }
        public int type_id { get; set; }
        public int brand_id { get; set; }
        public string location_car { get; set; }
        public DateTime year_production { get; set; }
        public float odo { get; set; }
        public float insurance_fee { get; set; }
        public int fuel { get; set; }
        public string description { get; set; }

        public float voucher { get; set; }
        public int number_seat { get; set; }
        public List<car_img> car_img { get; set; }
    }
    public class car_create
    {
        public int id { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public float price { get; set; }
        public byte isAuto { get; set; }
        public byte isRetail { get; set; }
        public int type_id { get; set; }
        public int brand_id { get; set; }
        public DateTime year_production { get; set; }
        public float odo { get; set; }
        public float insurance_fee { get; set; }
        public int fuel { get; set; }
        public string description { get; set; }
        public string car_name { get; set; }
        public string location_car { get; set; }
        public float voucher { get; set; }
        public int number_seat { get; set; }

    }
}
