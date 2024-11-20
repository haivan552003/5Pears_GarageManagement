using System;

namespace FE_User.Models
{
    public class guest_car
    {
        public int id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public float price { get; set; }
        public string status { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_guest { get; set; }
        public int id_emp { get; set; }
        public int id_cus { get; set; }
        public int id_car { get; set; }
    }

    public class guest_car_create
    {
        public int id { get; set; }
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int car_id { get; set; }
        public float price { get; set; }
        public float? pay_amount { get; set; }
        public string? bank_code { get; set; }
        public string? card_type { get; set; }
        public DateTime? pay_date { get; set; }
        public string? transaction_no { get; set; }
        public bool payment_method { get; set; }
        public DateTime? date_start { get; set; }
        public DateTime? date_end { get; set; }
    }
}
