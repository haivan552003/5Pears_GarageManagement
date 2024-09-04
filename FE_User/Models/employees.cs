using System;
using System.IO;
using System.Reflection;

namespace FE_User.Models
{
    public class employees
    {
        public int id_emp { get; set; }

        public string user_name { get; set; }
        public string pass_word { get; set; }
        public string full_name { get; set; }
        public DateTime birthday { get; set; }
        public string citizen_identity_img { get; set; }
        public string citizen_identity_number { get; set; }
        public byte gender { get; set; }
        public byte is_delete { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_role { get; set; }
    }
}
