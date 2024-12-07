using System;

namespace BE_API.ModelCustom
{
    public class calendarDriver
    {
        public int id { get; set; }
        public string fullname { get; set; }
        public DateTime date { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }
    }
    public class DriverDropdown
    {
        public int id { get; set; }
        public string name { get; set; }
        public string img { get; set; }
    }
}
