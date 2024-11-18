using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using Dapper;
using BE_API.ModelCustom;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using FirebaseAdmin.Auth;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly string _firebaseApiKey;

        public EmailController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlConnection");
            _configuration = configuration;
            _firebaseApiKey = configuration["Firebase:ApiKey"];

            // Khởi tạo Firebase Admin SDK nếu chưa được khởi tạo
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("D://management-c91fe-firebase-adminsdk-vnu5e-2653e72de7.json")
                });
            }
        }

        private IDbConnection DbConnection()
        {
            return new SqlConnection(_connectionString);
        }

        [HttpPost("send-verification-email")]
        public async Task<IActionResult> SendVerificationEmail([FromBody] EmailModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Email không hợp lệ.");
            }

            try
            {
                // Tạo user mới trong Firebase (nếu chưa tồn tại)
                UserRecordArgs userArgs = new UserRecordArgs()
                {
                    Email = model.Email,
                    EmailVerified = false
                };

                // Kiểm tra xem user đã tồn tại chưa
                try
                {
                    var existingUser = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(model.Email);
                    // Nếu user đã tồn tại, gửi email xác thực
                    await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(model.Email);
                }
                catch (FirebaseAuthException)
                {
                    // Nếu user chưa tồn tại, tạo user mới
                    var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);
                    // Sau đó gửi email xác thực
                    await FirebaseAuth.DefaultInstance.GenerateEmailVerificationLinkAsync(model.Email);
                }

                return Ok("Email xác thực đã được gửi thành công.");
            }
            catch (FirebaseAuthException ex)
            {
                return BadRequest($"Lỗi xác thực Firebase: {ex.Message}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] EmailVerificationModel model)
        {
            if (string.IsNullOrEmpty(model.EmailLink) || string.IsNullOrEmpty(model.Email))
            {
                return BadRequest("Liên kết xác thực hoặc email không hợp lệ.");
            }

            try
            {
                // Xác thực email thông qua Firebase Admin SDK
                var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(model.Email);

                // Kiểm tra trạng thái xác thực email
                if (user.EmailVerified)
                {
                    // Lưu email vào database
                    using (var connection = DbConnection())
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@Email", model.Email, DbType.String);
                        await connection.ExecuteAsync("sp_add_email", parameters, commandType: CommandType.StoredProcedure);
                    }

                    return Ok("Email đã được xác thực và lưu thành công.");
                }
                else
                {
                    return BadRequest("Email chưa được xác thực.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi khi xác thực email: {ex.Message}");
            }
        }
    }

    // Mẫu lớp để nhận thông tin email từ phía client
    public class EmailModel
    {
        public string Email { get; set; }
    }

    // Mẫu lớp để nhận thông tin xác thực email từ phía client
    public class EmailVerificationModel
    {
        public string Email { get; set; }
        public string EmailLink { get; set; }
    }
}
