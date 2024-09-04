﻿using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;

namespace BE_API.Models
{
    public class news
    {
        public int id_news { get; set; }
        public string news_img { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
        public int id_emp { get; set; }

    }
}
