using System.ComponentModel.DataAnnotations;

namespace FE_User.Models
{
    public class Change_pass
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu hiện tại")]

        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải ít nhất 8 ký tự")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
