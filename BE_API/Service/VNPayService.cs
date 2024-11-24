using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using BE_API.Models;
using Microsoft.Extensions.Configuration;

public class VNPayService
{
    private readonly IConfiguration _config;

    public VNPayService(IConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public string CreatePaymentUrl(string orderId, decimal amount, string orderInfo, string ipAddress, string returnUrl)
    {
        var vnpUrl = _config["VNPay:vnp_Url"];
        var vnpTmnCode = _config["VNPay:vnp_TmnCode"];
        var vnpHashSecret = _config["VNPay:vnp_HashSecret"];
        var vnpReturnUrl = _config["VNPay:vnp_ReturnUrl"];

        var vnpayData = new SortedList<string, string>
        {
            { "vnp_Version", "2.1.0" },
            { "vnp_Command", "pay" },
            { "vnp_TmnCode", vnpTmnCode },
            { "vnp_Amount", (amount * 100).ToString() }, // Chuyển đổi sang đơn vị đồng
            { "vnp_CurrCode", "VND" },
            { "vnp_TxnRef", orderId },
            { "vnp_OrderInfo", orderInfo },
            { "vnp_OrderType", "billpayment" },
            { "vnp_Locale", "vn" },
            { "vnp_ReturnUrl", returnUrl },
            { "vnp_IpAddr", ipAddress },
            { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture) }
        };

        var queryString = new StringBuilder();
        foreach (var kv in vnpayData)
        {
            if (queryString.Length > 0)
                queryString.Append("&");

            queryString.Append($"{kv.Key}={Uri.EscapeDataString(kv.Value)}");
        }

        var rawData = queryString.ToString();
        var vnpSecureHash = HmacSHA512(vnpHashSecret, rawData);
        var paymentUrl = $"{vnpUrl}?{rawData}&vnp_SecureHash={vnpSecureHash}";

        return paymentUrl;
    }

    public string HmacSHA512(string key, string data)
    {
        using (var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
    public (string OrderId, decimal Amount, string OrderInfo, string BankCode) GetPaymentDetailsFromUrl(string paymentUrl)
    {
        // Trích xuất chuỗi truy vấn từ URL
        var uri = new Uri(paymentUrl);
        var query = HttpUtility.ParseQueryString(uri.Query);

        // Lấy order ID từ các tham số truy vấn
        var orderId = query["vnp_TxnRef"];
        if (string.IsNullOrEmpty(orderId))
        {
            throw new ArgumentException("Không tìm thấy Order ID trong payment URL.");
        }

        // Lấy Amount và OrderInfo từ các tham số truy vấn
        var amountString = query["vnp_Amount"];
        var orderInfo = query["vnp_OrderInfo"];

        var bankCode = query["vnp_BankCode"];

        if (string.IsNullOrEmpty(amountString))
        {
            throw new ArgumentException("Không tìm thấy Amount trong payment URL.");
        }

        if (string.IsNullOrEmpty(orderInfo))
        {
            throw new ArgumentException("Không tìm thấy OrderInfo trong payment URL.");
        }

        // Chuyển đổi Amount từ chuỗi sang decimal
        decimal amount = decimal.Parse(amountString) / 100; // Chuyển đổi về đơn vị tiền tệ ban đầu

        return (orderId, amount, orderInfo, bankCode); // Trả về OrderId, Amount và OrderInfo
    }


}
