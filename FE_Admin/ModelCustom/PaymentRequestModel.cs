namespace FE_Admin.ModelCustom
{
    public class PaymentRequestModel
    {
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string OrderInfo { get; set; }
        public string returnUrl { get; set; }

    }
}
