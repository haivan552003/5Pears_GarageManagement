using System.Collections.Generic;

namespace BE_API.ModelCustom
{
    public class DetailedTripStatistics
    {
        public int TripID { get; set; }
        public string TripCode { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public int TicketsSold { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
