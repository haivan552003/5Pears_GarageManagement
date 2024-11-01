using System;
using System.Collections.Generic;

namespace BE_API.ModelCustom
{
    public class trip_detail
    {
        public string trip_detail_code { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string img_trip { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public string location_to { get; set; }
        public string location_from { get; set; }
        public float voucher { get; set; }
        public List<car_seat> car_seats { get; set; }

    }
    public class car_seat
    {
        public int id { get; set; }
        public string name { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        public int car_id { get; set; }
    }
}
