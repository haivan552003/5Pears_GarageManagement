using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace BE_API.Models
{
    public class guest_driver
    {
        public int id_guest_driver { get; set; }
        public int id_guest { get; set; }
        public int id_emp { get; set; }
        public int id_cus { get; set; }
        public int id_driver { get; set; }
    }
}
