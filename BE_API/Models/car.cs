using System;
using System.Drawing;

namespace BE_API.Models
{
    public class car
    {
        public int id { get; set; }
        public string img_name { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public string status { get; set; }
        public byte isAuto {  get; set; }
        public float price { get; set; }
        public int type_id { get; set; }
        public string type_name { get; set; }

        public int brand_id { get; set; }
        public string brand_name { get; set; }

        public DateTime year_production {  get; set; }
        public float odo {  get; set; }
        public float insurance_fee { get; set; }
    }
}
