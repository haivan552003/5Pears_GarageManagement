namespace FE_User.ModelCustom
{
    public class login_phone
    {
        public string phone_number { get; set; }
        public string password { get; set; }
        public int role_id { get; set; }
    }
    public class LoginResponseDto
    {
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
    }
}
