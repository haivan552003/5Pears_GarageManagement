using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;

namespace BE_API.Models
{
    public class role_task_role
    {
        public int id_role_task_role { get; set; }
        public DateTime date_create { get; set; }
        public DateTime date_update { get; set; }

        public int id_role_task { get; set; }
        public int id_role { get; set; }
    }
}
