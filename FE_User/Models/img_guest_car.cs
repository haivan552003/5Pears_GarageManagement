using System;

namespace FE_User.Models
{
    public class img_guest_car
    {
        public int id_img_guest_car { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_guest_car { get; set; }
    }
}
