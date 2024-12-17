using System;
using System.Collections.Generic;

namespace BE_API.ModelCustom
{
    public class CarRentalRequest
    {
        public DateTime DateStart { get; set; }
        public int CarId { get; set; }
    }
    public class DriverRentalRequest
    {
        public DateTime DateStart { get; set; }
        public int DriverId { get; set; }
        public int CarID { get; set; }
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
    public class AddImgGC
    {
        public string img_name { get; set; }
        public int guest_car_Id { get; set; }
    }
    public class GetImgGC
    {
        public int id { get; set; }
        public string img_car { get; set; }
        public int guest_car_id { get; set; }
    }
}
