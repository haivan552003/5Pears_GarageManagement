namespace FE_Admin.Models
{
    public class banner
    {
        public int id { get; set; }
        public string img_banner { get; set; }
        public string title { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int? emp_id { get; set; }
        public string fullname { get; set; }
    }
}
