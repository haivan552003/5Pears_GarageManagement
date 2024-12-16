namespace FE_Admin.Models
{
    public class guest_car
    {
        public int id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public float price { get; set; }
        public float return_money { get; set; }
        public string guest_car_code { get; set; }
        public int status { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public DateTime pay_date { get; set; }
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int car_id { get; set; }
        public string car_name { get; set; }
        public string emp_name { get; set; }
        public string cus_name { get; set; }
        public bool payment_method { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string car_number { get; set; }

    }

    public class guest_car_checkout
    {
        public int id { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public float price { get; set; }
        public float return_money { get; set; }
        public string guest_car_code { get; set; }
        public int status { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public DateTime pay_date { get; set; }
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int car_id { get; set; }
        public string car_name { get; set; }
        public string emp_name { get; set; }
        public string cus_name { get; set; }
        public bool payment_method { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string car_number { get; set; }

    }
    public class guest_car_create
    {
        public int id { get; set; }
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int car_id { get; set; }
        public float price { get; set; }
        public float deposit { get; set; }
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
