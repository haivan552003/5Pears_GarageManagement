namespace FE_Admin.ModelCustom
{
    public class statistics_car
    {
        public int id { get; set; }
        public string car_code { get; set; }
        public string car_name { get; set; }
        public string car_number { get; set; }

    }
    public class statistics_car_monthly
    {
            public int Month { get; set; }
            public int TripCount { get; set; }
            public decimal TotalRevenue { get; set; }
    }

}
