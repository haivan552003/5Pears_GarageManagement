using System;
using System.Drawing;

namespace BE_API.Models
{
    public class car
    {
        public int id_car { get; set; }
        public string car_number { get; set; }
        public string type { get; set; }
        public string brand { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public string status_vehicle_registration { get; set; }
        public string status { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
    }
}
