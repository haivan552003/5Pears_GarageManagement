namespace FE_Admin.Models
{
    public class employees
    {
        public int id { get; set; }  // Đảm bảo tên thuộc tính giống với tên cột trong SQL
        public string email { get; set; }
        public string emp_code { get; set; }
        public bool status { get; set; }

        public string password { get; set; }
        public string fullname { get; set; }
        public DateTime? birthday { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public bool gender { get; set; }

        public int id_role { get; set; }
    }
}
