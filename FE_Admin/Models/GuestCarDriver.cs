namespace FE_Admin.Models
{
    public class GuestCarDriver
    {
        public int id { get; set; }
        public string guest_car_code { get; set; }
        public int emp_id { get; set; }
        public string emp_name { get; set; }
        public int car_id { get; set; }
        public string car_name { get; set; }
        public int cus_id { get; set; }
        public string cus_name { get; set; }
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public float price { get; set; }
        public string bank_code { get; set; }
        public string card_type { get; set; }
        public DateTime? pay_date { get; set; }
        public string transaction_no { get; set; }
        public bool payment_method { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public int status { get; set; }
        public float deposit { get; set; }
        public string? customer_address { get; set; }

    }
    public class GuestCarDriver_create
    {
        public int emp_id { get; set; }
        public int cus_id { get; set; }
        public int driver_id { get; set; }
        public float price { get; set; }
        public int car_id { get; set; }
        public string bank_code { get; set; }
        public string card_type { get; set; }
        public DateTime? pay_date { get; set; }
        public string transaction_no { get; set; }
        public bool payment_method { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public float deposit { get; set; }
        public string? customer_address { get; set; }
    }
    public class GuestCarDriver_checkout
    {
        public int id { get; set; }
        public string guest_car_code { get; set; }
        public string guest_driver_code { get; set; }
        public int emp_id { get; set; }
        public string emp_name { get; set; }
        public int car_id { get; set; }
        public string car_name { get; set; }
        public int cus_id { get; set; }
        public string cus_name { get; set; }
        public int driver_id { get; set; }
        public string driver_name { get; set; }
        public string driver_phone { get; set; }
        public float price { get; set; }
        public string bank_code { get; set; }
        public string card_type { get; set; }
        public DateTime? pay_date { get; set; }
        public string transaction_no { get; set; }
        public bool payment_method { get; set; }
        public DateTime date_start { get; set; }
        public DateTime date_end { get; set; }
        public int status { get; set; }
        public float deposit { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string? customer_address { get; set; }

    }
    public class AddImgCarDriver
    {
        public string img_name { get; set; }
        public int guest_car_driver_Id { get; set; }
    }
    public class GetImgGcd
    {
        public int id { get; set; }
        public string img_car { get; set; }
        public int guest_car_driver_Id { get; set; }
    }
}
