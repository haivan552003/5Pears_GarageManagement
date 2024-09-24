using System.ComponentModel.DataAnnotations;

namespace FE_User.Models
{
    public class reset_pass
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mã OTP không hợp lệ")]

        public string Otp { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [RegularExpression(@"^(?=.*[0-9])(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Mật khẩu phải có ít nhất một chữ số và một ký tự đặc biệt.")]
        public string NewPassword { get; set; }
    }
}