//using Vonage.Request;
//using Vonage;
//using System;
//using Vonage.Verify;

//namespace BE_API.Service
//{
//    public class SmsService
//    {
//        private readonly VonageClient _client;

//        public SmsService()
//        {
//            // Khởi tạo VonageClient với API key và Secret từ cấu hình của bạn
//            var credentials = Credentials.FromApiKeyAndSecret("d83ab8d4", "GarRmn2h6ztQSWdv");
//            _client = new VonageClient(credentials);
//        }

//        public VerifyResponse SendOtp(string phoneNumber)
//        {
//            try
//            {
//                var request = new VerifyRequest()
//                {
//                    Brand = "Vonage",
//                    Number = phoneNumber
//                };

//                // Gửi yêu cầu xác minh OTP
//                var response = _client.VerifyClient.VerifyRequest(request);
//                Console.WriteLine("Yêu cầu OTP đã được gửi.");
//                return response;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi khi gửi OTP: {ex.Message}");
//                return null;
//            }
//        }

//        public VerifyResponse VerifyOtp(string code, string requestId)
//        {
//            try
//            {
//                var verifyRequest = new VerifyCheckRequest()
//                {
//                    Code = code,
//                    RequestId = requestId
//                };

//                // Kiểm tra OTP với mã đã nhập
//                var response = _client.VerifyClient.VerifyCheck(verifyRequest);
//                Console.WriteLine("Mã OTP đã được xác minh.");
//                return response;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi khi xác minh OTP: {ex.Message}");
//                return null;
//            }
//        }

//        public VerifyControlResponse CancelVerification(string requestId)
//        {
//            try
//            {
//                var controlRequest = new VerifyControlRequest()
//                {
//                    RequestId = requestId,
//                    Cmd = "cancel"
//                };

//                // Hủy yêu cầu xác minh OTP
//                var response = _client.VerifyClient.VerifyControl(controlRequest);
//                Console.WriteLine("Yêu cầu xác minh đã bị hủy.");
//                return response;
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Lỗi khi hủy yêu cầu xác minh: {ex.Message}");
//                return null;
//            }
//        }
//    }
//}
