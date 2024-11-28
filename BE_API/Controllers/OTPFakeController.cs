//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Data;
//using System.Threading.Tasks;
//using Vonage;
//using Vonage.Messaging;
//using Vonage.Request;
//using Dapper;
//using Microsoft.Data.SqlClient;

//namespace BE_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class OTPFakeController : ControllerBase
//    {
//        private readonly IMemoryCache _cache;
//        private readonly string _apiKey;
//        private readonly string _apiSecret;
//        private readonly string _connectionString;
//        private readonly ILogger<OTPFakeController> _logger;

//        public OTPFakeController(IConfiguration configuration, ILogger<OTPFakeController> logger, IMemoryCache memoryCache)
//        {
//            _apiKey = configuration["Vonage:ApiKey"];
//            _apiSecret = configuration["Vonage:ApiSecret"];
//            _connectionString = configuration.GetConnectionString("SqlConnection");
//            _logger = logger;
//            _cache = memoryCache;
//        }

//        // Gửi OTP
//        [HttpPost("send-otp1")]
//        public async Task<IActionResult> SendOtp1([FromBody] SendOtpRequest request)
//        {
//            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
//            {
//                return BadRequest("Số điện thoại không hợp lệ.");
//            }

//            try
//            {
//                // Khởi tạo thông tin xác thực
//                var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
//                var client = new VonageClient(credentials);

//                // Sinh mã OTP ngẫu nhiên
//                var otp = GenerateRandomOtp();

//                // Tạo yêu cầu gửi SMS
//                var sendSmsRequest = new SendSmsRequest
//                {
//                    From = "Vonage APIs",
//                    To = request.PhoneNumber,
//                    Text = $"Your OTP code is: {otp}"
//                };

//                // Gửi tin nhắn SMS
//                var response = client.SmsClient.SendAnSms(sendSmsRequest);

//                if (response.Messages != null && response.Messages[0].Status == "0") // Trạng thái "0" là thành công
//                {
//                    // Lưu OTP vào bộ nhớ cache
//                    _cache.Set(request.PhoneNumber, otp, TimeSpan.FromMinutes(5));

//                    // Thêm thông tin số điện thoại và OTP vào cơ sở dữ liệu
//                    await AddPhoneNumberToDatabase(request.PhoneNumber, otp);

//                    return Ok(new { Message = "Mã OTP đã được gửi qua SMS." });
//                }
//                else
//                {
//                    _logger.LogError($"Gửi OTP không thành công: {response.Messages[0].ErrorText}");
//                    return StatusCode(500, $"Lỗi: {response.Messages[0].ErrorText}");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi gửi OTP");
//                return StatusCode(500, "Lỗi khi gửi OTP. Vui lòng thử lại sau.");
//            }
//        }

//        // Xác minh OTP
//        [HttpPost("verify-otp1")]
//        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest3 request)
//        {
//            if (string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Otp))
//            {
//                return BadRequest("Số điện thoại hoặc mã OTP không hợp lệ.");
//            }

//            try
//            {
//                // Kiểm tra OTP từ bộ nhớ cache
//                if (_cache.TryGetValue(request.PhoneNumber, out string cachedOtp))
//                {
//                    if (cachedOtp == request.Otp)
//                    {
//                        // OTP hợp lệ, xóa OTP khỏi cache sau khi sử dụng
//                        _cache.Remove(request.PhoneNumber);

//                        return Ok(new { Message = "Xác minh OTP thành công." });
//                    }
//                    else
//                    {
//                        return BadRequest("Mã OTP không đúng.");
//                    }
//                }
//                else
//                {
//                    return BadRequest("OTP đã hết hạn hoặc không tồn tại.");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi xác minh OTP");
//                return StatusCode(500, "Đã xảy ra lỗi khi xác minh OTP. Vui lòng thử lại sau.");
//            }
//        }

//        // Tạo mã OTP ngẫu nhiên
//        private string GenerateRandomOtp()
//        {
//            var random = new Random();
//            return random.Next(1000, 9999).ToString(); // Sinh OTP gồm 4 chữ số
//        }

//        // Thêm số điện thoại và OTP vào cơ sở dữ liệu
//        private async Task AddPhoneNumberToDatabase(string phoneNumber, string otp)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                try
//                {
//                    await connection.OpenAsync();
//                    var parameters = new DynamicParameters();
//                    parameters.Add("@phone_number", phoneNumber, DbType.String);
//                    parameters.Add("@password", otp, DbType.String); // Lưu OTP vào cột `password`

//                    await connection.ExecuteAsync("sp_add_phone_number", parameters, commandType: CommandType.StoredProcedure);
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Lỗi khi lưu số điện thoại và OTP vào cơ sở dữ liệu");
//                    throw;
//                }
//            }
//        }

//        // Yêu cầu gửi OTP
//        public class SendOtpRequest
//        {
//            public string PhoneNumber { get; set; }
//        }

//        // Yêu cầu xác minh OTP
//        public class VerifyOtpRequest3
//        {
//            public string PhoneNumber { get; set; }
//            public string Otp { get; set; }
//        }
//    }
//}
