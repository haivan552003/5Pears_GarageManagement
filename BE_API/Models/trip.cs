using System;
using System.Collections.Generic;
using static Vonage.ProactiveConnect.Lists.SyncStatus;

namespace BE_API.Models
{
    public class trip_create
    {
        public string? img_trip { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int emp_create { get; set; }
        public bool status { get; set; }
        public bool is_return { get; set; }
    }
    public class trip_update
    {
        public string? img_trip { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public bool status { get; set; }
    }

    public class trip
    {
        public int id { get; set; }
        public string img_trip { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int emp_create { get; set; }
        public string trip_code { get; set; }
        public bool status { get; set; }
        public bool is_return { get; set; }
        public List<trip_detail> TripDetails { get; set; }
    }

    public class trip_detail
    {
        public int id { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public float sale { get; set; }
        public int trip_id { get; set; }
        public int car_id { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public int location_from_id { get; set; }
        public string trip_detail_code { get; set; }
        public int driver_id { get; set; }
        public int location_to_id { get; set; }
        public float distance { get; set; }
        public bool status { get; set; }
        public string car_number { get; set; }
        public string car_code { get; set; }
        public string car_name { get; set; }
        public string fullname { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public List<car_seat> car_seat { get; set; }

    }
    public class trip_detail_create
    {
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public int trip_id { get; set; }
        public int car_id { get; set; }
        public int location_from_id { get; set; }
        public int driver_id { get; set; }
        public int location_to_id { get; set; }
        public float distance { get; set; }
        public bool status { get; set; }
    }

    public class trip_detail_update
    {
        public int id { get; set; }
        public float price { get; set; }
        public float voucher { get; set; }
        public int car_id { get; set; }
        public int location_from_id { get; set; }
        public int driver_id { get; set; }
        public int location_to_id { get; set; }
        public float distance { get; set; }
        public bool status { get; set; }
    }

    public class search_trip
    {
        public string location_from { get; set; }
        public string location_to { get; set; }
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
