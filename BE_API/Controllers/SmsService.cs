using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Security.Cryptography;
using BE_API.ModelCustom;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailOtpService : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailOtpService> _logger;
        private static readonly ConcurrentDictionary<string, (string otp, DateTime expiry)> _otpStore = new ConcurrentDictionary<string, (string, DateTime)>();
        private static readonly ConcurrentDictionary<string, bool> _otpVerifiedStore = new ConcurrentDictionary<string, bool>();
        private readonly string _connectionString;

        public EmailOtpService(IConfiguration configuration, ILogger<EmailOtpService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("SqlConnection");
        }

        // Gửi OTP đến email
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] EmailOtpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return BadRequest("Email là bắt buộc.");
            }

            try
            {
                // Tạo OTP
                string otp = GenerateOtp();
                DateTime expiry = DateTime.UtcNow.AddMinutes(5); // OTP hết hạn sau 5 phút

                // Lưu OTP vào bộ nhớ tạm
                _otpStore[request.Email] = (otp, expiry);

                // Gửi OTP đến email
                await SendOtpEmail(request.Email, otp);

                return Ok("OTP đã được gửi đến email của bạn.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi gửi OTP");
                return StatusCode(500, "Lỗi khi gửi OTP. Vui lòng thử lại sau.");
            }
        }

        // Hàm phát sinh OTP
        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // OTP gồm 6 chữ số
        }

        // Hàm gửi email OTP
        private async Task SendOtpEmail(string email, string otp)
        {
            var smtpClient = new SmtpClient(_configuration["SmtpSettings:Server"])
            {
                Port = int.Parse(_configuration["SmtpSettings:Port"]),
                Credentials = new NetworkCredential(_configuration["SmtpSettings:FromEmail"], _configuration["SmtpSettings:FromPassword"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SmtpSettings:FromEmail"]),
                Subject = "Mã OTP của bạn",
                Body = $"Mã OTP của bạn là: {otp}",
                IsBodyHtml = false,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }

        // Phương thức xác minh OTP
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Otp))
            {
                return BadRequest("Email và OTP là bắt buộc.");
            }

            if (_otpStore.TryGetValue(request.Email, out var storedData))
            {
                var (storedOtp, expiry) = storedData;
                if (DateTime.UtcNow <= expiry && request.Otp == storedOtp)
                {
                    // OTP hợp lệ, đánh dấu email này đã xác minh OTP
                    _otpVerifiedStore[request.Email] = true;
                    return Ok(new { message = "Xác minh OTP thành công. Bạn có thể đặt lại mật khẩu.", isValid = true });
                }
            }

            return BadRequest(new { message = "OTP không hợp lệ hoặc đã hết hạn.", isValid = false });
        }

        // Phương thức đặt lại mật khẩu
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return BadRequest("Email và mật khẩu mới là bắt buộc.");
            }

            // Check if OTP has been verified for the user
            if (!_otpVerifiedStore.TryGetValue(request.Email, out bool otpVerified) || !otpVerified)
            {
                return BadRequest("OTP chưa được xác minh.");
            }

            try
            {
                // Hash the new password
                string hashedPassword = HashPassword(request.NewPassword);

                // Update the password in the customers table in the database
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Update query reflecting the correct database structure
                    int affectedRows = await connection.ExecuteAsync(
                        "UPDATE customers SET password = @Password WHERE username = @Email",  // Assume customer_email is the right column
                        new { Password = hashedPassword, Email = request.Email } // Matching parameters to the new query
                    );

                    if (affectedRows == 0)
                    {
                        return NotFound("Không tìm thấy người dùng.");
                    }
                }

                // Remove OTP verification flag after successful password reset
                _otpVerifiedStore.TryRemove(request.Email, out _);

                return Ok("Đặt lại mật khẩu thành công.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi xảy ra khi đặt lại mật khẩu");
                return StatusCode(500, "Có lỗi xảy ra khi đặt lại mật khẩu. Vui lòng thử lại sau.");
            }
        }


        // Hàm mã hóa mật khẩu
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
   
}
