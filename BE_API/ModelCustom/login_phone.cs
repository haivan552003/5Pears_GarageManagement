namespace BE_API.ModelCustom
{
    public class login_phone_user
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        //public string phonenumber { get; set; }
        //public string password { get; set; }
        //public string fullname { get; set; }
        //public int role_id { get; set; }
        //public int id { get; set; }
    }
    public class LoginResponseDto
    {
        public string PhoneNumber { get; set; }
        public string fullname { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }

}
