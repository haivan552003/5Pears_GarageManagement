using System;

namespace FE_User.Models
{
    public class img_car_driver
    {
        public int id_img_car_driver { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_guest_car_driver { get; set; }
    }
}
