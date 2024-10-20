using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;

namespace BE_API.Models
{
    public class car_brands
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }
    }
}
