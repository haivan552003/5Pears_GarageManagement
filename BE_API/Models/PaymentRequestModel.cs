namespace BE_API.Models
{
    public class PaymentRequestModel
    {
            public string OrderId { get; set; }
            public decimal Amount { get; set; }
            public string OrderInfo { get; set; }

    }
}
