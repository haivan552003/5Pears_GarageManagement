namespace FE_Admin.Models
{
    public class car
    {
        public int id { get; set; }
        public string car_number { get; set; }
        public string color { get; set; }
        public DateTime vehicle_registration_start { get; set; }
        public DateTime vehicle_registration_end { get; set; }
        public string status { get; set; }
        public byte is_auto { get; set; }
        public float price { get; set; }
        public int id_type { get; set; }
        public int id_brand { get; set; }
        public DateTime year_production { get; set; }
        public float odo { get; set; }
        public float insurance_fee { get; set; }
    }
}
