namespace BU.OnlineShop.PaymentService.API.Dtos
{
    public class CompleteInput
    {
        public string CardNumber { get; set; }

        public string CardName { get; set; }

        public string CardExpiration { get; set; }

        public float TotalAmount{ get; set; }
    }
}
