using System;

namespace BE_API.Models
{
    public class trip
    {
        public int id_trip { get; set; }
        public string img_trip { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string distance { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public byte is_delete { get; set; }
        public int emp_create { get; set; }
    }
}
