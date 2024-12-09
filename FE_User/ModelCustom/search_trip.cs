namespace FE_User
{
    public class search_trip
    {
        public string location_to { get; set; }
        public string location_from { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_return { get; set; }
        public bool is_return { get; set; }
    }
    public class search_trip_data
    {
        public int id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public float price { get; set; }
        public IEnumerable<search_tripdetail> search_tripdetail { get; set; }
    }
    public class search_tripdetail
    {
        public int trip_id { get; set; }
        public int id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string location_to { get; set; }
        public string location_to_address { get; set; }
        public string location_from { get; set; }
        public string location_from_address { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
    }
}
