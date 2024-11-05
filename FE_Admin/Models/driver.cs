using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FE_Admin.Models
{
    public class driver
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Ảnh tài xế là bắt buộc.")]
        public string img_driver { get; set; }
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Họ và tên phải từ 3 đến 100 ký tự")]
        public string fullname { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Giá không hợp lệ.")]
        public float price { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Voucher không hợp lệ.")]
        public float voucher { get; set; }
        [Required(ErrorMessage = "Ngày sinh là bắt buộc.")]
        [DataType(DataType.Date)]
        public DateTime birthday { get; set; }
        [Required(ErrorMessage = "Ảnh CCCD (mặt trước) là bắt buộc.")]
        public string citizen_identity_img1 { get; set; }
        [Required(ErrorMessage = "Ảnh CCCD (mặt sau) là bắt buộc.")]
        public string citizen_identity_img2 { get; set; }
        [Required(ErrorMessage = "Số CCCD là bắt buộc.")]
        [RegularExpression(@"^\d{9}|\d{12}$", ErrorMessage = "Số CCCD không hợp lệ.")]
        public string citizen_identity_number { get; set; }
        [Required(ErrorMessage = "Ảnh GPLX (mặt trước) là bắt buộc.")]
        public string driver_license_img1 { get; set; }
        [Required(ErrorMessage = "Ảnh GPLX (mặt sau) là bắt buộc.")]
        public string driver_license_img2 { get; set; }
        [Required(ErrorMessage = "Số GPLX là bắt buộc.")]
        [RegularExpression(@"^\d{9}$", ErrorMessage = "Số GPLX phải có 9 chữ số.")]
        public string driver_license_number { get; set; }
        [Range(0, 1, ErrorMessage = "Giới tính không hợp lệ.")]
        public byte gender { get; set; }
        [Required(ErrorMessage = "Số điện thoại là bắt buộc.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})\b", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string phonenumber { get; set; }
        [Required(ErrorMessage = "Địa chỉ là bắt buộc.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Địa chỉ phải từ 5 đến 200 ký tự")]
        public string address { get; set; }
        [Range(0, 1, ErrorMessage = "Trạng thái không hợp lệ.")]

        public byte status { get; set; }

        public string driver_code { get; set; }
        public string email { get; set; }
        public int class_driver_license { get; set; }
        public string password { get; set; }

    }
}
