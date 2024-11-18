//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using Vonage;
//using Vonage.Request;
//using Vonage.Verify;
//using Dapper;
//using Microsoft.Data.SqlClient;
//using System;
//using System.Data;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using Microsoft.Extensions.Caching.Memory;

//namespace BE_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class PhoneNumberController : ControllerBase
//    {
//        private readonly IMemoryCache _cache;

//        private readonly string _apiKey;
//        private readonly string _apiSecret;
//        private readonly string _connectionString;
//        private readonly ILogger<PhoneNumberController> _logger;
//        private static readonly Dictionary<string, string> _requestIdCache = new();


//        public PhoneNumberController(IConfiguration configuration, ILogger<PhoneNumberController> logger, IMemoryCache memoryCache)
//        {
//            _apiKey = configuration["Vonage:ApiKey"];
//            _apiSecret = configuration["Vonage:ApiSecret"];
//            _connectionString = configuration.GetConnectionString("SqlConnection");
//            _logger = logger;
//            _cache = memoryCache;
//        }

//        // Gửi OTP
//        [HttpPost("send-otp")]
//        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
//        {
//            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
//            {
//                return BadRequest("Số điện thoại không hợp lệ.");
//            }

//            try
//            {
//                var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
//                var client = new VonageClient(credentials);

//                var verifyRequest = new VerifyRequest()
//                {
//                    Brand = "Vonage",
//                    Number = request.PhoneNumber
//                };
//                var response = client.VerifyClient.VerifyRequest(verifyRequest);

//                if (response.Status == "0")
//                {
//                    // Lưu RequestId vào Cache
//                    _cache.Set(request.PhoneNumber, response.RequestId, TimeSpan.FromMinutes(5));

//                    // Gọi hàm lưu số điện thoại vào CSDL
//                    await AddPhoneNumberToDatabase(request.PhoneNumber);

//                    return Ok(new { Message = "Mã OTP đã được gửi qua SMS." });
//                }
//                else
//                {
//                    _logger.LogError($"Gửi OTP không thành công: {response.ErrorText}");
//                    return StatusCode(500, $"Lỗi: {response.ErrorText}");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi gửi OTP");
//                return StatusCode(500, "Lỗi khi gửi OTP. Vui lòng thử lại sau.");
//            }
//        }



//        // Xác minh OTP
//        [HttpPost("verify-otp")]
//        public IActionResult VerifyOtp([FromBody] VerifyOtpRequest1 request)
//        {
//            if (string.IsNullOrWhiteSpace(request.PhoneNumber) || string.IsNullOrWhiteSpace(request.Otp))
//            {
//                return BadRequest("Số điện thoại và OTP là bắt buộc.");
//            }

//            try
//            {
//                // Truy xuất RequestId từ Cache
//                if (!_cache.TryGetValue(request.PhoneNumber, out string requestId) || string.IsNullOrWhiteSpace(requestId))
//                {
//                    return BadRequest("RequestId không tồn tại hoặc đã hết hạn.");
//                }

//                var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
//                var client = new VonageClient(credentials);

//                var verifyCheckRequest = new VerifyCheckRequest()
//                {
//                    RequestId = requestId,
//                    Code = request.Otp
//                };
//                var response = client.VerifyClient.VerifyCheck(verifyCheckRequest);

//                if (response.Status == "0")
//                {
//                    return Ok("Số điện thoại đã được xác minh và lưu thành công.");
//                }
//                else
//                {
//                    return BadRequest($"Lỗi: {response.ErrorText}");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi xác minh OTP");
//                return StatusCode(500, "Lỗi khi xác minh OTP. Vui lòng thử lại sau.");
//            }
//        }


//        // Hủy yêu cầu xác minh
//        [HttpPost("cancel-verify-request")]
//        public IActionResult CancelVerifyRequest([FromBody] CancelVerifyRequest request)
//        {
//            if (string.IsNullOrWhiteSpace(request.RequestId))
//            {
//                return BadRequest("RequestId là bắt buộc.");
//            }

//            try
//            {
//                var credentials = Credentials.FromApiKeyAndSecret(_apiKey, _apiSecret);
//                var client = new VonageClient(credentials);

//                var verifyControlRequest = new VerifyControlRequest()
//                {
//                    RequestId = request.RequestId,
//                    Cmd = "cancel"
//                };
//                var response = client.VerifyClient.VerifyControl(verifyControlRequest);

//                if (response.Status == "0")
//                {
//                    return Ok("Yêu cầu xác minh đã được hủy.");
//                }
//                else
//                {
//                    return BadRequest($"Lỗi: {response.ErrorText}");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Lỗi khi hủy yêu cầu xác minh");
//                return StatusCode(500, "Lỗi khi hủy yêu cầu xác minh. Vui lòng thử lại sau.");
//            }
//        }

//        private async Task AddPhoneNumberToDatabase(string phoneNumber)
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                try
//                {
//                    await connection.OpenAsync();
//                    var parameters = new DynamicParameters();
//                    parameters.Add("@phone_number", phoneNumber, DbType.String);
//                    parameters.Add("@password", phoneNumber, DbType.String);

//                    await connection.ExecuteAsync("sp_add_phone_number", parameters, commandType: CommandType.StoredProcedure);
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Lỗi khi lưu số điện thoại vào cơ sở dữ liệu");
//                    throw;
//                }
//            }
//        }
//    }

//    public class SendOtpRequest
//    {
//        public string PhoneNumber { get; set; }
//    }

//    public class VerifyOtpRequest1
//    {
//        public string PhoneNumber { get; set; }
//        public string Otp { get; set; }
//        public string RequestId { get; set; }
//    }

//    public class CancelVerifyRequest
//    {
//        public string RequestId { get; set; }
//    }
//}
