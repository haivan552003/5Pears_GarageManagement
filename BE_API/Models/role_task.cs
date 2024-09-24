using System;
using System.ComponentModel.DataAnnotations;

namespace BE_API.Models
{
    public class role_task
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
    }
}
