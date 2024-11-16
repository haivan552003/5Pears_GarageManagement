namespace BE_API.ModelCustom
{
    public class statistics_driver_by_trip
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public int TotalTrips { get; set; }
        public string Driver_code { get; set; }
    }
    public class statistics_driver_by_trip_monly
    {
        public int Month { get; set; }
        public int Total { get; set; }
    }
}
