﻿namespace BE_API.ModelCustom
{
    public class login
    {
        public string email { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public int role_id { get; set; }
        public int id { get; set; }
    }

    public class login_phone
    {
        public string phone_number { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public int role_id { get; set; }
        public int id { get; set; }
    }
    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }
    }
}
