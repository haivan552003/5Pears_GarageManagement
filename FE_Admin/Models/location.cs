namespace FE_Admin.Models
{
    public class location
    {
        public int id { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string phone_number { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public byte is_delete { get; set; }
        public string location_code { get; set; }
        public Boolean status { get; set; }
    }
}
