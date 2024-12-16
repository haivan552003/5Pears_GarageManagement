using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace BE_API.Models
{
    public class guest_trip
    {
        public int id { get; set; }
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int trip_id { get; set; }
        public int trip_detail_id { get; set; }
        public int location_from_id { get; set; }
        public int location_to_id { get; set; }
        public int car_id { get; set; }
        public string guest_trip_code { get; set; }
        public string trip_detail_code { get; set; }
        public string emp_name { get; set; }
        public string cus_name { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
        public DateTime pay_date { get; set; }
        public float price { get; set; }
        public float return_money { get; set; }
        public int status { get; set; }
        public bool payment_method { get; set; }
        public DateTime date_create { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string car_name { get; set; }
        public string car_number { get; set; }

        public string from { get; set; }
        public string to { get; set; }

        public List<guest_trip_child> guest_trip_child { get; set; }
    }
    public class guest_trip_child
    {
        public int id { get; set; }
        public string guest_trip_code { get; set; }
        public string location_from { get; set; }
        public string location_to { get; set; }
        public string seat_name { get; set; }
        public string col { get; set; }
        public string row { get; set; }
        public float price { get; set; }
        public DateTime time_start { get; set; }
        public DateTime time_end { get; set; }
    }
    public class guest_trip_create
    {
        public int trip_detail_id { get; set; }
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public float price { get; set; }
        public string? bank_code { get; set; }
        public string? card_type { get; set; }
        public DateTime? pay_date { get; set; }
        public string? transaction_no { get; set; }
        public bool payment_method { get; set; }
        public int? parent_id { get; set; }
        public int? car_seat_id { get; set; }
    }
    public class guest_trip_update
    {
        public bool? payment_method { get; set; }
        public string bank_code { get; set; }
        public string card_type { get; set; }
        public string? transaction_no { get; set; }
        public string? transaction_status { get; set; }

    }
}
