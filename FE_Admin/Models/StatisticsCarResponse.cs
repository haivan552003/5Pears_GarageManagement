namespace FE_Admin.Models
{
    public class StatisticsCarResponse
    {
        public int id { get; set; }
        public string CarCode { get; set; }
        public string CarName { get; set; }
    }
    public class MonthlyStatistic
    {
        public int Month { get; set; }
        public int TripCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
