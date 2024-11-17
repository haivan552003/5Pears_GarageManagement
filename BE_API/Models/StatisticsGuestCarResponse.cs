namespace BE_API.Models
{
    public class StatisticsGuestCarResponse
    {
        public int Month { get; set; }
        public string CarCode { get; set; }
        public string CarName { get; set; }
        public int TripCount { get; set; }
        public decimal TotalRevenue { get; set; }
        public string GuestCarCodes { get; set; }
    }
}
