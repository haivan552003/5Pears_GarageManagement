using System;

namespace FE_User.Models
{
    public class news
    {
        public int id { get; set; }
        public string news_img { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_emp { get; set; }
        public Boolean status { get; set; }
        public string fullname { get; set; }

    }
}
