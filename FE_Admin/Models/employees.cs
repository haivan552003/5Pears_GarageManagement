namespace FE_Admin.Models
{
    public class employees
    {
        public int id { get; set; }
        public string email { get; set; }
        public string emp_code { get; set; }
        public bool status { get; set; }
        public string img_emp { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string role_name { get; set; }
        public string phone_number { get; set; }
        public DateTime birthday { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public bool gender { get; set; }
        public DateTime date_create { get; set; }
        public int role_id { get; set; }
    }
}
