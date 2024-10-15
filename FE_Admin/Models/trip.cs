namespace FE_Admin.Models
{
    public class trip
    {
        public int id { get; set; }
        public string img_trip { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public DateTime? date_create { get; set; }
        public DateTime? date_update { get; set; }
        public byte is_delete { get; set; }
        public int emp_create { get; set; }
        public string trip_code { get; set; }
        public bool status { get; set; }
        public bool is_return { get; set; }
    }
}
