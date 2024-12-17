namespace FE_Admin.Models
{
    public class guest_driver
    {
        public int id { get; set; }
        public int id_emp { get; set; }
        public int id_cus { get; set; }
        public int id_driver { get; set; }
        public string driver_name { get; set; }
        public string driver_phone { get; set; }
        public string cus_name { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string guest_driver_code { get; set; }
        public float price { get; set; }
        public float deposit { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
    }

    public class guest_driver_create
    {
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int driver_id { get; set; }
        public float price { get; set; }
        public string? bank_code { get; set; }
        public string? card_type { get; set; }
        public DateTime? pay_date { get; set; }
        public string? transaction_no { get; set; }
        public bool? payment_method { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public float deposit { get; set; }
    }
}
