namespace FE_User.ModelCustom
{
    public class RateModel
    {
        public int id { get; set; }
        public int rate { get; set; }
        public string rate_content { get; set; }
    }
    public class GuestCarDriverHistory
    {
        public int id { get; set; }
        public float price { get; set; }
        public string Car_Name { get; set; }
        public string Car_Code { get; set; }
        public string Car_Number { get; set; }
        public string Color { get; set; }
        public string FullName { get; set; }
        public string Driver_Code { get; set; }
        public string EmpName { get; set; }
        public int status { get; set; }
        public int rate { get; set; }
        public string rate_content { get; set; }
    }

    public class GuestCarHistory
    {
        public int id { get; set; }
        public float Price { get; set; }
        public string guest_car_code { get; set; }
        public string car_code { get; set; }
        public string car_name { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public string fullname { get; set; }
        public int status { get; set; }
        public int rate { get; set; }
        public string rate_content { get; set; }
    }

    public class GuestDriverHistory
    {
        public int id { get; set; }
        public decimal Price { get; set; }
        public string guest_driver_code { get; set; }
        public string fullname { get; set; }
        public string driver_code { get; set; }
        public string address { get; set; }
        public string EmpName { get; set; }
        public int status { get; set; }
        public int rate { get; set; }
        public string rate_content { get; set; }
    }

    public class GuestTripHistory
    {
        public int id { get; set; }
        public decimal Price { get; set; }
        public string guest_trip_code { get; set; }
        public int car_seat_id { get; set; }
        public string fullname { get; set; }
        public string driver_code { get; set; }
        public string address { get; set; }
        public string car_code { get; set; }
        public string car_name { get; set; }
        public string car_number { get; set; }
        public string name { get; set; }
        public int status { get; set; }
        public int rate { get; set; }
        public string rate_content { get; set; }
    }
}
