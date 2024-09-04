using System;

namespace FE_User.Models
{
    public class guest_car_driver
    {
        public int id_guest_car_driver { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public float price { get; set; }
        public string status { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_guest { get; set; }
        public int id_emp { get; set; }
        public int id_cus { get; set; }
        public int id_driver { get; set; }
        public int id_car { get; set; }
    }
}
