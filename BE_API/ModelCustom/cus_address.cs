using System;

namespace BE_API.ModelCustom
{
    public class cus_address
    {
        public int id { get; set; }
        public string address { get; set; }
        public int id_cus { get; set; }
        public byte type { get; set; }
        public bool status {  get; set; }
        //public DateTime date_create { get; set; }
        //public DateTime date_update { get; set; }
    }
}
