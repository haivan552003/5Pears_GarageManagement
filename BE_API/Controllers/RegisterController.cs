using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Data;
using System.Threading.Tasks;
using System;
using System.Net.Mail;
using System.Net;
using BE_API.ModelCustom;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
    private readonly string _connectionString;
    private readonly IConfiguration _configuration;
    private static readonly ConcurrentDictionary<string, (RegisterRequest userInfo, string otp, DateTime expiry)> _registrationStore = new();

    public RegisterController(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("SqlConnection");
    }

    [HttpPost("validate-registration")]
    public async Task<IActionResult> ValidateRegistration([FromBody] RegisterRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.PhoneNumber) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest(new { message = "Vui lòng điền đầy đủ thông tin" });
            }

            // Kiểm tra email và số điện thoại đã tồn tại
            using (var connection = new SqlConnection(_connectionString))
            {
                var emailExists = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM dbo.customers WHERE email = @Email AND is_delete = 0",
                    new { Email = request.Email }
                );

                if (emailExists > 0)
                {
                    return BadRequest(new { message = "Email đã được đăng ký" });
                }

                var phoneExists = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM dbo.customers WHERE phone_number = @PhoneNumber AND is_delete = 0",
                    new { PhoneNumber = request.PhoneNumber }
                );

                if (phoneExists > 0)
                {
                    return BadRequest(new { message = "Số điện thoại đã được đăng ký" });
                }
            }

            // Tạo và gửi OTP
            var otp = new Random().Next(100000, 999999).ToString();

            // Lưu thông tin người dùng và OTP
            _registrationStore[request.Email] = (request, otp, DateTime.UtcNow.AddMinutes(5));

            // Gửi OTP qua email
            try
            {
                var smtpClient = new SmtpClient(_configuration["SmtpSettings:Server"])
                {
                    Port = int.Parse(_configuration["SmtpSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["SmtpSettings:FromEmail"],
                        _configuration["SmtpSettings:FromPassword"]
                    ),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["SmtpSettings:FromEmail"]),
                    Subject = "Mã OTP Đăng Ký Tài Khoản",
                    Body = $@"
                        <h2>Xác nhận đăng ký tài khoản</h2>
                        <p>Mã OTP của bạn là: <strong>{otp}</strong></p>
                        <p>Mã này sẽ hết hạn sau 5 phút.</p>
                        <p>Vui lòng không chia sẻ mã này với người khác.</p>
                    ",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(request.Email);

                await smtpClient.SendMailAsync(mailMessage);

                return Ok(new { message = "Mã OTP đã được gửi đến email của bạn" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi khi gửi email", error = ex.Message });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi kiểm tra thông tin đăng ký", error = ex.Message });
        }
    }

    // API 2: Xác thực OTP và hoàn tất đăng ký
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOTPAndRegister([FromBody] OtpVerificationRequest request)
    {
        try
        {
            // Kiểm tra OTP
            if (!_registrationStore.TryGetValue(request.Email, out var storedData))
            {
                return BadRequest(new { message = "Không tìm thấy thông tin đăng ký" });
            }

            var (userInfo, storedOtp, expiry) = storedData;

            if (DateTime.UtcNow > expiry)
            {
                _registrationStore.TryRemove(request.Email, out _);
                return BadRequest(new
                {
                    message = "Mã OTP đã hết hạn",
                    requireNewOtp = true
                });
            }

            if (request.Otp != storedOtp)
            {
                return BadRequest(new
                {
                    message = "Mã OTP không đúng",
                    remainingAttempts = 3 // Có thể thêm logic đếm số lần thử
                });
            }

            // Kiểm tra lại một lần nữa trước khi đăng ký
            using (var connection = new SqlConnection(_connectionString))
            {
                var exists = await connection.QueryFirstOrDefaultAsync<int>(
                    "SELECT COUNT(*) FROM dbo.customers WHERE (email = @Email OR phone_number = @PhoneNumber) AND is_delete = 0",
                    new { Email = userInfo.Email, PhoneNumber = userInfo.PhoneNumber }
                );

                if (exists > 0)
                {
                    _registrationStore.TryRemove(request.Email, out _);
                    return BadRequest(new { message = "Email hoặc số điện thoại đã được đăng ký" });
                }

                // Hash mật khẩu
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userInfo.Password);

                // Đăng ký tài khoản
                var parameters = new DynamicParameters();
                parameters.Add("@Username", userInfo.Email);
                parameters.Add("@Password", hashedPassword);
                parameters.Add("@Fullname", userInfo.FullName);
                parameters.Add("@Birthday", userInfo.Birthday);
                parameters.Add("@Gender", userInfo.Gender);
                parameters.Add("@PhoneNumber", userInfo.PhoneNumber);
                parameters.Add("@IDRole", 2);

                await connection.ExecuteAsync("sp_register", parameters, commandType: CommandType.StoredProcedure);
            }

            // Xóa thông tin đăng ký tạm thời
            _registrationStore.TryRemove(request.Email, out _);

            return Ok(new { message = "Đăng ký thành công" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi trong quá trình đăng ký", error = ex.Message });
        }
    }

    // API phụ: Gửi lại OTP nếu cần
    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOTP([FromBody] ResendOtpRequest request)
    {
        try
        {
            if (!_registrationStore.TryGetValue(request.Email, out var storedData))
            {
                return BadRequest(new { message = "Không tìm thấy thông tin đăng ký" });
            }

            var (userInfo, _, _) = storedData;

            // Tạo OTP mới
            var newOtp = new Random().Next(100000, 999999).ToString();
            _registrationStore[request.Email] = (userInfo, newOtp, DateTime.UtcNow.AddMinutes(5));

            // Gửi OTP mới qua email
            var smtpClient = new SmtpClient(_configuration["SmtpSettings:Server"])
            {
                Port = int.Parse(_configuration["SmtpSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["SmtpSettings:FromEmail"],
                    _configuration["SmtpSettings:FromPassword"]
                ),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SmtpSettings:FromEmail"]),
                Subject = "Mã OTP Mới - Đăng Ký Tài Khoản",
                Body = $@"
                    <h2>Mã OTP mới cho đăng ký tài khoản</h2>
                    <p>Mã OTP mới của bạn là: <strong>{newOtp}</strong></p>
                    <p>Mã này sẽ hết hạn sau 5 phút.</p>
                    <p>Vui lòng không chia sẻ mã này với người khác.</p>
                ",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(request.Email);

            await smtpClient.SendMailAsync(mailMessage);

            return Ok(new { message = "Mã OTP mới đã được gửi đến email của bạn" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi gửi lại OTP", error = ex.Message });
        }
    }
}

