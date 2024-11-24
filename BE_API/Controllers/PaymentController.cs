using BE_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Linq;

namespace BE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly VNPayService _vnpayService;
        private readonly IConfiguration _config; // Thêm biến _config

        public PaymentController(VNPayService vnpayService, IConfiguration config) // Nhận IConfiguration thông qua constructor
        {
            _vnpayService = vnpayService;
            _config = config; // Khởi tạo biến _config
        }

        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] PaymentRequestModel model)
        {
            // Kiểm tra các thuộc tính của model
            if (model == null || string.IsNullOrEmpty(model.OrderId) || model.Amount <= 0 || string.IsNullOrEmpty(model.ReturnUrl))
            {
                return BadRequest("Thông tin thanh toán không hợp lệ.");
            }

            var paymentUrl = _vnpayService.CreatePaymentUrl(model.OrderId, model.Amount, model.OrderInfo,
         Request.HttpContext.Connection.RemoteIpAddress.ToString(), model.ReturnUrl);

            return Ok(new { paymentUrl });
        }


        [HttpGet("vnpay_return")]
        public IActionResult VNPayReturn()
        {
            var vnpHashSecret = _config["VNPay:vnp_HashSecret"];
            var vnpayData = Request.Query;

            var vnp_SecureHash = vnpayData["vnp_SecureHash"];
            var sortedData = new SortedList<string, string>();

            foreach (var item in vnpayData)
            {
                if (!item.Key.Equals("vnp_SecureHash", StringComparison.OrdinalIgnoreCase))
                {
                    sortedData.Add(item.Key, item.Value);
                }
            }

            var rawData = string.Join("&", sortedData.Cast<KeyValuePair<string, string>>()
                .Select(x => $"{x.Key}={x.Value}"));
            var secureHash = _vnpayService.HmacSHA512(vnpHashSecret, rawData);

            if (secureHash == vnp_SecureHash)
            {
                var orderId = vnpayData["vnp_TxnRef"];
                var amount = vnpayData["vnp_Amount"];
                var transactionStatus = vnpayData["vnp_ResponseCode"];
                var bankCode = vnpayData["vnp_BankCode"];
                var paymentDate = vnpayData["vnp_PayDate"];

                // Thông tin bổ sung từ tham số
                var bankTranNo = vnpayData["vnp_BankTranNo"];
                var cardType = vnpayData["vnp_CardType"];

                // Lưu thông tin thanh toán vào cơ sở dữ liệu nếu cần thiết
                // ...

                return Ok(new
                {
                    OrderId = orderId,
                    Amount = amount,
                    TransactionStatus = transactionStatus,
                    BankCode = bankCode,
                    PaymentDate = paymentDate,
                    BankTransactionNo = bankTranNo,
                    CardType = cardType
                });
            }
            else
            {
                return BadRequest("Invalid Signature");
            }
        }


        [HttpGet("get-order-from-url")]
        public IActionResult GetOrderFromUrl([FromQuery] string paymentUrl)
        {
            if (string.IsNullOrEmpty(paymentUrl))
            {
                return BadRequest("Payment URL không hợp lệ.");
            }

            try
            {
                // Trích xuất thông tin từ paymentUrl
                var paymentDetails = _vnpayService.GetPaymentDetailsFromUrl(paymentUrl);

                // Trả về tất cả thông tin
                return Ok(paymentDetails);
            }
            catch (Exception ex)
            {
                return BadRequest($"Lỗi: {ex.Message}");
            }
        }



    }
}
