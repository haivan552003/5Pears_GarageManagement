using System;

namespace BE_API.ModelCustom
{
    public class Email_service
    {
    }
    public class EmailCheckRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public byte Gender { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class EmailOtpRequest
    {
        public string Email { get; set; }
    }
    public class VerifyOtpRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
    public class OtpVerificationRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
    public class ResendOtpRequest
    {
        public string Email { get; set; }
    }
}
