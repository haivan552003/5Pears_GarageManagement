namespace FE_Admin.ModelCustom
{
    public class dropdown
    {
        public int id { get; set; }
        public string name { get; set; }
        public string img { get; set; }
    }
    public class DriverDropdown
    {
        public int id { get; set; }
        public string name { get; set; }
        public string img { get; set; }
    }

    public class DriverCalendar
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public DateTime date { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }
    }
}
