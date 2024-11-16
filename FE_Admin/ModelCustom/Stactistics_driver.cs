namespace FE_Admin.ModelCustom
{
    public class Stactistics_driver
    {
    }
    public class MonthlyStatistic
    {
        public int Month { get; set; }
        public int Total { get; set; }
        public decimal Revenue { get; set; }
    }
    public class statistics_driver_by_GCD
    {
        public int id { get; set; }
        public string FullName { get; set; }
        public string Driver_code { get; set; }
    }
    public class statistics_driver_by_GCD_monly
    {
        public int Month { get; set; }
        public int Total { get; set; }

        public decimal Revenue { get; set; }
    }


    public class statistics_driver_by_trip
    {
        public int Id { get; set; }
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
